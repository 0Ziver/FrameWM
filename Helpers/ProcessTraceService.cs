using Frame.Exec;
using System.Diagnostics;
using System.Management;

namespace Frame.Helpers
{
    public class ProcessTraceService : IInit, IOnExit
    {
        private const string ProcessStartTrace = "SELECT * FROM Win32_ProcessStartTrace";
        private const string ProcessStopTrace = "SELECT * FROM Win32_ProcessStopTrace";

        public static event Action<Process> OnProcessStop;
        public static event Action<Process> OnProcessStart;


        private ManagementEventWatcher
            startProcessWatcher,
            stopProcessWatcher;

        private WqlEventQuery
            startProcessQuery,
            stopProcessQuery;

        public void Init()
        {
            startProcessQuery = new(ProcessStartTrace);
            using (startProcessWatcher = new(startProcessQuery))
            {
                startProcessWatcher.EventArrived += StartWatcher;
                startProcessWatcher.Start();
            }
            stopProcessQuery = new(ProcessStopTrace);
            using (stopProcessWatcher = new(stopProcessQuery))
            {
                stopProcessWatcher.EventArrived += StopWatcher;
                stopProcessWatcher.Start();
            }

            /*ThreadPool.QueueUserWorkItem(_ =>
            {
                InstallWatcher(queryP: ProcessStartTrace, wql: startProcessQuery, watcher: startProcessWatcher);
                InstallWatcher(queryP: ProcessStopTrace, wql: stopProcessQuery, watcher: stopProcessWatcher);
            });*/
        }

        /*private void InstallWatcher(string queryP, WqlEventQuery wql, ManagementEventWatcher watcher)
        {
            wql = new(queryP);
            using (watcher = new(wql))
            {
                watcher.EventArrived += ProcessWatcher;
                watcher.Start();
            }
        }*/

        private void UnistallWatcher(params ManagementEventWatcher[] watchers)
        {
            watchers[0].EventArrived -= StartWatcher;
            watchers[1].EventArrived -= StartWatcher;
            /*foreach (var watcher in watchers)
            {
                watcher.EventArrived -= ProcessWatcher;
                watcher.Stop();
            }*/
        }

        private int PID(EventArrivedEventArgs e)
        {
            return Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
        }


        void StopWatcher(object sender, EventArrivedEventArgs e)
        {
            string eventType = e.NewEvent.ClassPath.ClassName;
            PropertyDataCollection properties = e.NewEvent.Properties;
            string processName;
            int processId;
            processName = properties["ProcessName"].Value.ToString();
            processId = Convert.ToInt32(properties["ProcessID"].Value);
            Console.WriteLine($"Process close. Name: {processName}, ID: {processId}");
        }

        void StartWatcher(object sender, EventArrivedEventArgs e)
        {
            string eventType = e.NewEvent.ClassPath.ClassName;
            PropertyDataCollection properties = e.NewEvent.Properties;
            string processName;
            int processId;
            processName = properties["ProcessName"].Value.ToString();
            processId = Convert.ToInt32(properties["ProcessID"].Value);
            Console.WriteLine($"Process start. Name: {processName}, ID: {processId}");
        }

        /*private void ProcessWatcher(object sender, EventArrivedEventArgs e)
        {
            string eventType = e.NewEvent.ClassPath.ClassName;
            PropertyDataCollection properties = e.NewEvent.Properties;
            string processName;
            int processId;

            switch (eventType)
            {
                case "Win32_ProcessStartTrace":
                    processName = properties["ProcessName"].Value.ToString();
                    processId = Convert.ToInt32(properties["ProcessID"].Value);
                    Console.WriteLine($"Process close. Name: {processName}, ID: {processId}");

                    break;
                case "Win32_ProcessStopTrace":
                    processName = properties["ProcessName"].Value.ToString();
                    processId = Convert.ToInt32(properties["ProcessID"].Value);
                    Console.WriteLine($"Process close. Name: {processName}, ID: {processId}");

                    break;
            }
        }*/


        public void OnExit()
        {
            UnistallWatcher(startProcessWatcher, stopProcessWatcher);
        }
    }
}