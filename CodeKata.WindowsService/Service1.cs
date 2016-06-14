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
        public static ConcurrentDictionary<int, SubmittedTask> _concurrentDirtyTasks = new ConcurrentDictionary<int, SubmittedTask>();

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

            try
            {
                // If we were using stored procs instead of models
                //var connectionString = ConfigurationManager.ConnectionStrings["CodeKataContext"].ConnectionString;
                //SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(connectionString);
                //_sqlConnectionString = sqlBuilder.ConnectionString;
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
            GetSubmittedTasksTimer.Interval = 10000; // Every 10 seconds
            GetSubmittedTasksTimer.Start();
        
            // todo: connection pool cleanup task?

            // Dirty Updater
            Task.Run(() =>
            {
                while (_applicationIsRunning)
                {
                    SubmittedTask task;
                    if (_concurrentDirtyTasks.Any() && _concurrentDirtyTasks.TryRemove(_concurrentDirtyTasks.First().Key, out task))
                    {
                        Task.Run(() =>
                        {
                            switch (task.Status)
                            {
                                case TaskStatus.Queued:
                                    task.Status = TaskStatus.Processing;
                                    task.StartDateTime = DateTime.UtcNow;
                                    task.UpdateExistingTask();

                                    // "Process" Task
                                    task.Status = ProcessSubmittedTask(task);
                                    task.EndDateTime = DateTime.UtcNow;
                                    task.UpdateExistingTask();

                                    break;
                                case TaskStatus.Processing: // Should never hit
                                    task.Status = TaskStatus.Error;
                                    task.EndDateTime = DateTime.UtcNow;
                                    break;
                                default: // Finished or Error
                                    Console.WriteLine("Task is in finished or error status");
                                    break;
                            }

                            SubmittedTask throwAway;
                            _concurrentSubmittedTasks.TryRemove(task.Id, out throwAway); // We use this to stop duplicates from entering dirty queue
                        });
                    }

                    Thread.Sleep(100);
                }
            });
        }

        // Method called when timer elapses
        async void GetSubmittedTasksTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetSubmittedTasksTimer.Stop(); // Pause while running

            try
            {
                await ImportSubmittedTasks();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Unable to import newly submitted tasks.");
                EventLog.WriteEntry(_eventLogSource, ex.Message, EventLogEntryType.Error);
            }

            GetSubmittedTasksTimer.Start();
        }

        public static async Task ImportSubmittedTasks()
        {
            try
            {
                // Note: kind of pointless to offload this now, but could be useful if we begin
                // pulling in from multiple states/statuses and need to massage data
                var allQueuedTasks = SubmittedTask.GetTasksByStatus(TaskStatus.Queued);
                foreach (SubmittedTask task in allQueuedTasks)
                {
                    await QueueSubmittedTask(task);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Unable to query DB for submitted tasks.");
                EventLog.WriteEntry(_eventLogSource, ex.Message, EventLogEntryType.Error);
            }
        }

        private static Task QueueSubmittedTask(SubmittedTask task)
        {
            return Task.Run(() =>
            {
                try
                {
                    //SubmittedTask oldTask; If exists, could pull out to see if varies
                    if (!_concurrentSubmittedTasks.ContainsKey(task.Id))
                    {
                        _concurrentSubmittedTasks.TryAdd(task.Id, task);
                        _concurrentDirtyTasks.TryAdd(task.Id, task);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: Unable to process a submitted task.");
                    EventLog.WriteEntry(_eventLogSource, ex.Message, EventLogEntryType.Error);
                }
            });
        }

        private static TaskStatus ProcessSubmittedTask(SubmittedTask task)
        {
            var oneSecond = 1000;
            var thirtySeconds = 30000;

            // Simulate "Processing"
            Random rnd = new Random();
            Thread.Sleep(rnd.Next(oneSecond, thirtySeconds));

            return TaskStatus.Finished;
        }

        protected override void OnStop()
        {
            // Stop any timers
            GetSubmittedTasksTimer.Stop();
            _applicationIsRunning = false;
        }
    }
}
