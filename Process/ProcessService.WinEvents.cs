namespace Frame.Process
{
    using static Helpers.LibImports;

    public partial class ProcessService
    {
        internal class WinEvents
        {
            public static event Action<System.Diagnostics.Process?> OnChangeFocus;
            public static event Action<System.Diagnostics.Process?> OnWindowCreate;
            public static event Action<System.Diagnostics.Process?> OnWindowMinimized;

            private WinEvent
                _eFocus,
                _eCreate,
                _eMinimize;

            private Filter _filter;

            public WinEvents()
            {
                _filter = new Filter();

                #region WinEvents

                /*----------------------------------------------------------------------------------------------------*/
                _eFocus = new WinEvent(
                    (uint)EventObject.FOREGROUND,
                    (uint)EventObject.FOREGROUND,
                    (uint)EventContext.OUT,
                    out bool foreground);
                _eFocus.OnNewEvent += hwnd =>
                {
                    if (!foreground) return;
                    OnChangeFocus?.Invoke(_filter.FilterProcesses(hwnd));
                };
                /*----------------------------------------------------------------------------------------------------*/
                _eCreate = new WinEvent(
                    (uint)EventObject.CREATE,
                    (uint)EventObject.CREATE,
                    (uint)EventContext.OUT,
                    out bool create);
                _eCreate.OnNewEvent += hwnd =>
                {
                    if (!create) return;
                    OnWindowCreate?.Invoke(_filter.FilterProcesses(hwnd));
                };
                /*----------------------------------------------------------------------------------------------------*/
                _eMinimize = new WinEvent(
                    (uint)EventObject.MINIMIZEEND,
                    (uint)EventObject.MINIMIZEEND,
                    (uint)EventContext.OUT,
                    out bool minimized);
                _eMinimize.OnNewEvent += hwnd =>
                {
                    if (!minimized) return;
                    OnWindowMinimized?.Invoke(_filter.FilterProcesses(hwnd));
                };
                /*----------------------------------------------------------------------------------------------------*/

                #endregion
            }
        }
    }


    internal class WinEvent : IDisposable
    {
        public event Action<IntPtr> OnNewEvent;
        private readonly uint _eventMin;
        private readonly uint _eventMax;
        private readonly uint _dwFlags;
        private WinEventDelegate _wed;

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
            _wed = WinEventProc;
            _hook = SetWinEventHook(
                _eventMin,
                _eventMax,
                IntPtr.Zero,
                _wed,
                0,
                0,
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