using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeKata.Domain.Models;
using TaskStatus = CodeKata.Domain.Models.TaskStatus;

namespace CodeKata.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private static bool _applicationIsRunning = false;
        private static string _eventLogSource = "CodeKata";
        private static string _sqlConnectionString;

        public static System.Timers.Timer GetSubmittedTasksTimer = new System.Timers.Timer();
        public static ConcurrentDictionary<int, SubmittedTask> _concurrentSubmittedTasks = new ConcurrentDictionary<int, SubmittedTask>(); 

        public Service1()
        {
            InitializeComponent();
        }

        [Conditional("DEBUG_SERVICE")]
        private static void DebugMode()
        {
            Debugger.Break();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            DebugMode();

            // Make sure we have an EventLog to write to
            if (!EventLog.SourceExists(_eventLogSource))
            {
                EventSourceCreationData data = new EventSourceCreationData(_eventLogSource, "Application");
                EventLog.CreateEventSource(data);
            }

            // todo: Turn on a monitoring service?

            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["CodeKataContext"].ConnectionString;
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(connectionString);
                _sqlConnectionString = sqlBuilder.ConnectionString;
                StartTimers();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Unable to connect to database");
                EventLog.WriteEntry(_eventLogSource, ex.Message, EventLogEntryType.Error);
                this.Stop();
            }
        }

        public void StartTimers()
        {
            // Generally a good idea to put a cap in place
            ThreadPool.SetMaxThreads(50, 50);
            _applicationIsRunning = true;

            //
            GetSubmittedTasksTimer.Elapsed += GetSubmittedTasksTimer_Elapsed;
            GetSubmittedTasksTimer.Interval = 5000; // Every 10 seconds
            GetSubmittedTasksTimer.Start();
        
            // todo: connection pool cleanup task?

            // Forever running threads
            Task.Run(() =>
            {
                var hello = 0;
                while (_applicationIsRunning)
                {
                    // Spin up tasks to update dirty task entries in DB
                    hello++;
                    Thread.Sleep(1000);
                }
            });
        }

        // Method called when timer elapses
        async void GetSubmittedTasksTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetSubmittedTasksTimer.Stop(); // Pause while running

            try
            {
                // todo: Update statistics
                await ImportNewSubmittedTasks();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Unable to import newly submitted tasks.");
                EventLog.WriteEntry(_eventLogSource, ex.Message, EventLogEntryType.Error);
            }

            GetSubmittedTasksTimer.Start();
        }

        public static async Task ImportNewSubmittedTasks()
        {
            try
            {
                var allQueuedTasks = SubmittedTask.GetTasksByStatus(TaskStatus.Queued);
                // Update statistics with jobs in queue for "being worked on" ???
                foreach (SubmittedTask task in allQueuedTasks)
                {
                    await ProcessSubmittedTask(task);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Unable to query DB for submitted tasks.");
                EventLog.WriteEntry(_eventLogSource, ex.Message, EventLogEntryType.Error);
            }
        }

        private static Task ProcessSubmittedTask(SubmittedTask task)
        {
            return Task.Run(() =>
            {
                try
                {
                    // Background permenant thread updating dirty dictionary
                    // Questions? Can there ever be overlap or concurrency issues?

                    // todo: ALL OF THIS
                    // Update to "processing"
                    // Update ConcurrentDictionary
                    // Add to DirtyConcurrentDictionary

                    // AWAIT :: Kick off any child actions?

                    // Update to "finished"
                    // Update ConcurrentDictionary
                    // Add to DirtyConcurrentDictionary

                    // On update loop, when saving Dirty item in "finished" status,
                    // We can remove from both dictionaries

                    SubmittedTask oldTask;
                    // If exists, update
                    if (_concurrentSubmittedTasks.ContainsKey(task.Id))
                    {
                        if (_concurrentSubmittedTasks.TryGetValue(task.Id, out oldTask))
                            _concurrentSubmittedTasks.TryUpdate(task.Id, task, oldTask);
                    }
                    // If new, add
                    else
                    {
                        _concurrentSubmittedTasks.TryAdd(task.Id, task);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: Unable to process a submitted task.");
                    EventLog.WriteEntry(_eventLogSource, ex.Message, EventLogEntryType.Error);
                }
            });
        }

        protected override void OnStop()
        {
            // Stop any timers
            GetSubmittedTasksTimer.Stop();
            _applicationIsRunning = false;
            // Kill monitor service
        }
    }
}
