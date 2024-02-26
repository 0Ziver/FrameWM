namespace Frame.Process
{
    using static Helpers.LibImports;

    public partial class ProcessService
    {
        internal struct Filter
        {
            private HashSet<IntPtr> DuplicateFilter;

            public Filter()
            {
                DuplicateFilter = new HashSet<IntPtr>(2048);
            }

            public System.Diagnostics.Process? FilterProcesses(IntPtr hwnd)
            {
                GetWindowThreadProcessId(hwnd, out int pid);
                var process = System.Diagnostics.Process.GetProcessById(pid);

                RECT rect;
                GetWindowRect(hwnd, out rect);
                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;

                // Проверяем, что у процесса есть основное окно и его можно двигать
                if (process.MainWindowHandle == IntPtr.Zero || !IsWindowVisible(hwnd))
                    return null;

                // Проверяем размер окна
                if (width <= 50 || height <= 50)
                    return null;
                

             

                return process;
            }
        }
    }
}