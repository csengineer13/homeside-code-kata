using System;
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

namespace CodeKata.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private static bool _applicationIsRunning = false;
        private static string _eventLogSource = "CodeKata";
        private static string _sqlConnectionString;

        public static System.Timers.Timer GetSubmittedTasksTimer = new System.Timers.Timer();

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
                // StartTimers();
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
        }

        // Method called when timer elapses
        async void GetSubmittedTasksTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetSubmittedTasksTimer.Stop(); // Pause while running

            try
            {
                // Update statistics
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
                // Find tasks that are marked as submitted
                // Update statistics with jobs in queue for "being worked on" ???
                // await Pass to "ProcessTask"
                    // In ConcDict? Try Update
                    // Not? Try Add
                    // If different/dirty? Add to Update Queue
                        // Don't add if in "end state"
                        // Simply remove as non-working

            }
            catch (Exception ex)
            {
                
            }
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
