using System.Diagnostics;
using System.Text;
using static Frame.Helpers.LibImports;

namespace Frame.Helpers
{
    internal class ProcessStateHook
    {
        private HashSet<IntPtr> FilterContainer = new(2000);


        WinEventDelegate _WinEventDelegate;
        public ProcessStateHook()
        {
            _WinEventDelegate = new(ChangeFocusProc);
            SetWinEventHook(
                0x0003,
                0x0016,
                IntPtr.Zero,
                _WinEventDelegate,
                0,
                0,
                (uint)EVENT_CONTEXT.OUT);

        }

        private void Filter(IntPtr hwnd)
        {
            if (IsWindow(hwnd) && IsWindowVisible(hwnd) && IsWindowEnabled(hwnd) && !FilterContainer.Contains(hwnd))
            {
                int processId;
                int threadId = GetWindowThreadProcessId(hwnd, out processId);

                Process process = Process.GetProcessById(processId);

                StringBuilder windowText = new StringBuilder(256);
                GetWindowText(hwnd, windowText, windowText.Capacity);

                RECT windowRect;
                if (GetWindowRect(hwnd, out windowRect))
                {
                    // Получение стилей расширенного окна
                    uint exStyle = GetWindowLong(hwnd, -20);

                    // Проверка наличия флага WS_EX_TOOLWINDOW
                    bool isToolWindow = (exStyle & 0x00000080) != 0;

                    if (!isToolWindow && (windowRect.Right - windowRect.Left) > 100 && (windowRect.Bottom - windowRect.Top) > 100)
                    {
                        FilterContainer.Add(hwnd);
                        Console.WriteLine($"\nFiltered Window:\nHWND: {hwnd}\nPID: {processId}\nName: {windowText}\nThread: {threadId}\n");
                    }
                }
            }
        }
        // Task Switching, Search , Start
        // Network Connections , Windows Shell Experience Host,
        //  Windows Ink Workspace

        public void ChangeFocusProc(IntPtr hWinEventHook, uint eventType,
        IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (eventType == (uint)EVENT.EVENT_SYSTEM_FOREGROUND)
            {
                ConsoleLog(hwnd, "Focus");
            }
            if (eventType == 0x0016) // Minimize
            {
                ConsoleLog(hwnd, "Minimize");
            }
        }



        private void ConsoleLog(nint HWND, string from)
        {
            int pid;
            GetWindowThreadProcessId(HWND, out pid);

            if (Process.GetProcesses().Any(p => p.Id == pid))
            {
                Process p = Process.GetProcessById(pid);

                /*StringBuilder windowText = new StringBuilder(256);
                GetWindowText(HWND, windowText, windowText.Capacity);*/
                uint exStyle = GetWindowLong(HWND, -20);
                bool isToolWindow = (exStyle & 0x00000080) != 0;

                if (IsWindow(HWND))
                {
                    Console.WriteLine($"{from}: \n NAME: {p.ProcessName} PID: {p.MainWindowHandle} \n");
                }
            }
        }

        /*public void ChangeFocusProc(IntPtr hWinEventHook, uint eventType,
         IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (eventType == (uint)EVENT.EVENT_SYSTEM_FOREGROUND)
            {
                if (!FilterContainer.Contains(hwnd))
                {
                    FilterContainer.Add(hwnd);
                    int pid;
                    StringBuilder Name = new StringBuilder(256);
                    GetWindowText(hwnd, Name, Name.Capacity);
                    var t = GetWindowThreadProcessId(hwnd, out pid);
                    Console.WriteLine($"\n FROM FOCUS EVENT");
                    Console.WriteLine($"____________________________________________________________________");
                    Console.WriteLine($" \n HWND: {hwnd} \n PID: {pid} \n NAME: {Name} \n THREAD: {t}");
                    Console.WriteLine($"____________________________________________________________________");
                }
            }
            if (eventType == (uint)EVENT.EVENT_OBJECT_CREATE)
            {
                if (!FilterContainer.Contains(hwnd))
                {
                    FilterContainer.Add(hwnd);
                    int pid;
                    StringBuilder Name = new StringBuilder(256);
                    GetWindowText(hwnd, Name, Name.Capacity);
                    var t = GetWindowThreadProcessId(hwnd, out pid);
                    Console.WriteLine($"\n FROM CREATE EVENT");
                    Console.WriteLine($"____________________________________________________________________");
                    Console.WriteLine($" \n HWND: {hwnd} \n PID: {pid} \n NAME: {Name} \n THREAD: {t}");
                    Console.WriteLine($"____________________________________________________________________");
                }
            }
        }*/

    }
}
