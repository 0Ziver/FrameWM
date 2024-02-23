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
                DuplicateFilter = new HashSet<IntPtr>(128);
            }

            public System.Diagnostics.Process? FilterProcesses(IntPtr hwnd)
            {
                GetWindowThreadProcessId(hwnd, out int pid);
                var process = System.Diagnostics.Process.GetProcessById(pid);
                if (process.MainWindowHandle != IntPtr.Zero
                    && !DuplicateFilter.Contains(hwnd)
                    && System.Diagnostics.Process.GetProcesses().All(p => p.Id != pid) &&
                    process.MainWindowTitle.Length == 0) return null;
                return process;
            }
        }
    }
}
