using System.Diagnostics;
using System.Management;

namespace Frame.ProcessSercive
{
    using static Process;
    public partial class ProcessesSercive
    {
        internal class Trace
        {
            private const string stopQuery = "SELECT * FROM Win32_ProcessStopTrace";
            private const string startQuery = "SELECT * FROM Win32_ProcessStartTrace";
            public static event Action<Process> OnProcessClose;
            private HashSet<Process> Processes = new(4096);
            private readonly Mutex mutex = new();
            public Trace()
            {

                Shapshot();


                var _strF = new Filter();

                var startProcessWatcher = new ProcessWatcher(startQuery, pid =>
                {
                    Processes.Add(_strF.FilterProcesses(processID: pid));
                });

                var stopProcessWatcher = new ProcessWatcher(stopQuery, pid =>
                {
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        var tempP = new List<Process>();

                        mutex.WaitOne();
                        tempP.AddRange(Processes.Where(process => process != null && process.Id == pid));
                        foreach (var process in tempP)
                        {
                            if (process != null)
                            {
                                OnProcessClose?.Invoke(process);
                                Processes.Remove(process);
                            }
                        }
                        mutex.ReleaseMutex();
                    });
                });
            }


            private void Shapshot()
            {
                var shanpshop = GetProcesses();
                var _filter = new Filter();

                foreach (var process in shanpshop)
                {
                   // Console.WriteLine($"Unfiltered process = {process.ProcessName}");
                    var p = _filter.FilterProcesses(processID: process.Id);
                    if (p == null) return;
                    Console.WriteLine($"Filtered process = {p.ProcessName}");
                    Processes.Add(p);
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