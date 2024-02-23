using System.Management;

namespace Frame.Process
{
    public partial class ProcessService
    {
        internal class Trace
        {
            public static Watcher PStart;
            public static Watcher PStop;

            public Trace()
            {
                PStart = new Watcher(Watcher.WType.ProcessStartTrace);
                PStop = new Watcher(Watcher.WType.ProcessStopTrace);
            }

            public class Watcher : IDisposable
            {
                private Filter _filter;

                public struct WType
                {
                    public const string ProcessStartTrace = "SELECT * FROM Win32_ProcessStartTrace";
                    public const string ProcessStopTrace = "SELECT * FROM Win32_ProcessStopTrace";
                }


                public event Action<System.Diagnostics.Process?> OnProcessCreated;
                public event Action<System.Diagnostics.Process?> OnProcessClosed;

                private readonly ManagementEventWatcher _watcher;


                public Watcher(string query)
                {
                    _filter = new Filter();

                    var eventQuery = new WqlEventQuery(query);
                    try
                    {
                        using (_watcher = new ManagementEventWatcher(eventQuery))
                        {
                            _watcher.EventArrived += Result;
                            Task.Run(() => { _watcher.Start(); }).Wait();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                private void Result(object sender, EventArrivedEventArgs e)
                {
                    if (e.NewEvent == null) return;

                    /*var hwnd = System.Diagnostics.Process.GetProcessById(processId: process).MainWindowHandle;
                    var postFilter = _filter.FilterProcesses(hwnd).MainWindowHandle;
                    GetWindowThreadProcessId(postFilter, out int pid);
                    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(pid);*/

                    // Console.WriteLine($"Start: {p.ProcessName}");


                    /*if (e.NewEvent.ClassPath.ClassName == "Win32_ProcessStartTrace")
                    {
                        var process = Convert.ToString(e.NewEvent.Properties["ProcessName"].Value);

                        Console.WriteLine($"Start: {process}");
                    }
                    else if (e.NewEvent.ClassPath.ClassName == "Win32_ProcessStopTrace")

                    {
                        var process = Convert.ToString(e.NewEvent.Properties["ProcessName"].Value);

                        Console.WriteLine($"Stop: {process}");
                    }*/


                    switch (e.NewEvent.ClassPath.ClassName)
                    {
                        case "Win32_ProcessStartTrace":
                            var STRP = Convert.ToString(e.NewEvent.Properties["ProcessName"].Value);
                            Console.WriteLine($"Start: {STRP}");
                            //  OnProcessCreated?.Invoke(_filter.FilterProcesses(hwnd));
                            break;
                        case "Win32_ProcessStopTrace":
                            var STPP = Convert.ToString(e.NewEvent.Properties["ProcessName"].Value);

                            Console.WriteLine($"Stop: {STPP}");
                            // OnProcessClosed?.Invoke(_filter.FilterProcesses(hwnd));
                            break;
                    }
                }



                public void Dispose()
                {
                    _watcher.EventArrived -= Result;
                    _watcher.Stop();
                    _watcher.Dispose();
                }
            }
        }
    }
}
