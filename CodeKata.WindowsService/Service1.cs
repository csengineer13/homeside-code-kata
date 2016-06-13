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
using System.Threading.Tasks;

namespace CodeKata.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private static string _eventLogSource = "CodeKata";
        private static string _sqlConnectionString;

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

        protected override void OnStop()
        {
            // Stop any timers
            // Kill monitor service
        }
    }
}
