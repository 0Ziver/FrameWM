using Frame.Exec;
using System.Diagnostics;
using System.Management;

namespace Frame.Helpers
{
    public class ProcessTraceService : IInit, IOnExit
    {

        public void Init()
        {
        //    Watcher watcher = new Watcher(Watcher.wType.ProcessStartTrace);
        }
        public void OnExit()
        {
        }
    }

    public struct Watcher : IDisposable
    {
        public struct wType
        {
            public const string ProcessStartTrace = "SELECT * FROM Win32_ProcessStartTrace";
            public const string ProcessStopTrace = "SELECT * FROM Win32_ProcessStopTrace";
        }
        public event Action<EventArrivedEventArgs> OnMessegeFromWMI;
        private readonly ManagementEventWatcher _watcher;
        private WqlEventQuery _eventQuery;


        public Watcher(string query)
        {
            _eventQuery = new WqlEventQuery(query);
            try
            {
                using (_watcher = new ManagementEventWatcher(_eventQuery))
                {
                    _watcher.EventArrived += Result;
                    _watcher.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void Result(object sender, EventArrivedEventArgs e)
        {
            OnMessegeFromWMI?.Invoke(e);
        }

        public void Dispose()
        {
            _watcher.EventArrived -= Result;
            _watcher.Stop();
            _watcher.Dispose();
        }
    }

}
