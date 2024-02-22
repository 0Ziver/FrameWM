using System.Diagnostics.CodeAnalysis;
using System.Management;
using Frame.Exec;

namespace Frame.Process
{
    using static Helpers.LibImports;

    public partial class ProcessService : IInit
    {
        private ProcessChangeState _processChangeState;
        private ProcessTrace _processTrace;

        public void Init()
        {
            _processTrace = new ProcessTrace();
            _processChangeState = new ProcessChangeState();
        }
    }

    public partial class ProcessService
    {
        public class ProcessChangeState
        {
            public static event Action<System.Diagnostics.Process?> OnChangeFocus;
            public static event Action<System.Diagnostics.Process?> OnWindowCreate;
            public static event Action<System.Diagnostics.Process?> OnWindowMinimized;
            private WinEvent eF, eC, eM;

            private Filter _filter;

            public ProcessChangeState()
            {
                _filter = new Filter();

                #region WinEvents

                /*----------------------------------------------------------------------------------------------------*/
                eF = new WinEvent((uint)EventObject.FOREGROUND, (uint)EventObject.FOREGROUND,
                    (uint)EventContext.OUT, out bool foreground);
                eF.OnNewEvent += hwnd =>
                {
                    if (!foreground) return;
                    OnChangeFocus?.Invoke(_filter.FilterProcesses(hwnd));
                };
                /*----------------------------------------------------------------------------------------------------*/
                eC = new WinEvent((uint)EventObject.CREATE, (uint)EventObject.CREATE,
                    (uint)EventContext.OUT, out bool create);
                eC.OnNewEvent += hwnd =>
                {
                    if (!create) return;
                    OnWindowCreate?.Invoke(_filter.FilterProcesses(hwnd));
                };
                /*----------------------------------------------------------------------------------------------------*/
                eM = new WinEvent((uint)EventObject.MINIMIZEEND, (uint)EventObject.MINIMIZEEND,
                    (uint)EventContext.OUT, out bool minimized);
                eM.OnNewEvent += hwnd =>
                {
                    if (!minimized) return;
                    OnWindowMinimized?.Invoke(_filter.FilterProcesses(hwnd));
                };
                /*----------------------------------------------------------------------------------------------------*/

                #endregion
            }
        }
    }

    public partial class ProcessService
    {
        internal class WinEvent : IDisposable
        {
            public event Action<IntPtr> OnNewEvent;
            private readonly uint _eventMin;
            private readonly uint _eventMax;
            private readonly uint _dwFlags;
            private WinEventDelegate wed;

            private IntPtr _hook;

            public WinEvent(uint eventMin, uint eventMax, uint dwFlags, out bool result)
            {
                _eventMin = eventMin;
                _eventMax = eventMax;
                _dwFlags = dwFlags;
                result = InstallHook();
            }

            private bool InstallHook()
            {
                wed = WinEventProc;
                _hook = SetWinEventHook(
                    _eventMin,
                    _eventMax,
                    IntPtr.Zero,
                    wed,
                    0, 0,
                    _dwFlags);
                return _hook != IntPtr.Zero;
            }

            private void WinEventProc(IntPtr hWinEventHook, uint eventType,
                IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
            {
                OnNewEvent.Invoke(hwnd);
            }

            public void Dispose()
            {
                UnhookWinEvent(_hook);
            }
        }
    }

    public partial class ProcessService
    {
        internal struct Filter
        {
            private HashSet<IntPtr> DuplicateFilter;

            public Filter()
            {
                DuplicateFilter = new HashSet<IntPtr>(128);
            }

            public System.Diagnostics.Process? FilterProcesses(IntPtr hwnd)
            {
                GetWindowThreadProcessId(hwnd, out int pid);
                var process = System.Diagnostics.Process.GetProcessById(pid);
                if (process.MainWindowHandle != IntPtr.Zero
                    && !DuplicateFilter.Contains(hwnd)
                    && System.Diagnostics.Process.GetProcesses().All(p => p.Id != pid)) return null;
                return process;
            }
        }
    }

    public partial class ProcessService
    {
        public class ProcessTrace
        {
            [NotNull] public static Watcher PStart;
            [NotNull] public static Watcher PStop;

            public ProcessTrace()
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


                public event Action<System.Diagnostics.Process> OnNewProcessCreated;
                public event Action<string> OnNewProcessName;

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

                    var processName = Convert.ToString(e.NewEvent.Properties["ProcessID"].Value);

                  //  int process = Convert.ToInt32(e.NewEvent.Properties["ProcessName"].Value);
                    Console.WriteLine(processName);
                   // OnNewProcessName?.Invoke(processName);
                    
                  //  var process = System.Diagnostics.Process.GetProcessById(processId);

                   // OnNewProcessCreated?.Invoke(_filter.FilterProcesses(process.MainWindowHandle)!);
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