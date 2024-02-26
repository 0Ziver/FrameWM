using Frame.Helpers;
using System.Management;
using System.Runtime.InteropServices;

namespace Frame.Process
{
    public partial class ProcessService
    {
        internal class Trace
        {
            private const string stopQuery = "SELECT * FROM Win32_ProcessStopTrace";
            private const string startQuery = "SELECT * FROM Win32_ProcessStartTrace";
            public static event Action<System.Diagnostics.Process> OnProcessClose;
            private HashSet<System.Diagnostics.Process> Processes = new(1024);


            public Trace()
            {
                Shapshot();
                var startProcessWatcher = new ProcessWatcher(startQuery, pid => { });
                var stopProcessWatcher = new ProcessWatcher(stopQuery, pid =>
                {
                    foreach (var process in Processes)
                    {
                        if (pid == process.Id)
                        {
                            Console.WriteLine("Collision!");
                        }
                    }
                });
            }

            private void Shapshot()
            {
                var shanpshop = System.Diagnostics.Process.GetProcesses();
                foreach (var process in shanpshop)
                {
                    Processes.Add(process);
                }
            }
        }


        public class ProcessWatcher : IDisposable
        {
            private readonly Action<int> _action;
            private readonly ManagementEventWatcher _watcher;

            public ProcessWatcher(string query, Action<int> action)
            {
                _action = action;

                var evQuery = new WqlEventQuery(query);
                _watcher = new ManagementEventWatcher(evQuery);
                _watcher.EventArrived += Result;

                try
                {
                    _watcher.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            private void Result(object sender, EventArrivedEventArgs e)
            {
                int name = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
                _action?.Invoke(name);
            }

            public void Dispose()
            {
                _watcher.EventArrived -= Result;
                _watcher.Dispose();
            }
        }
    }
}