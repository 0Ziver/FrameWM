namespace Frame.ProcessSercive
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using static Helpers.LibImports;
    using static System.Diagnostics.Process;
    public partial class ProcessesSercive
    {
        internal struct Filter
        {
            private Mutex mutex = new();
            private ConcurrentDictionary<int, Process> processes = new ConcurrentDictionary<int, Process>();
            private HashSet<IntPtr> DuplicateFilter;
            readonly string[] blacklist = [
                "msedgewebview2",
                "BackgroundDownload",
                "svchost",
                "explorer",
                "QRSL",
                "backgroundTaskHost",
                "RuntimeBroker",
                "VBCSCompiler",
                "conhost"];


            public Filter()
            {
                DuplicateFilter = new HashSet<IntPtr>(2048);
            }

            /* public Process? FilterProcesses(IntPtr hwnd = default, int processID = default)
             {
                 int pid;
                 switch (processID)
                 {
                     case > 0:
                         pid = GetProcesses().ContainsKey(processID) ? processID : -1;
                         break;
                     default:
                         try
                         {
                             GetWindowThreadProcessId(hwnd, out pid);
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine($"An error occurred while getting process ID from window handle: {ex.Message}");
                             pid = -1;
                         }
                         if (pid == 0 || !GetProcesses().ContainsKey(pid))
                         {
                             pid = -1;
                         }
                         break;
                 }
                 if (pid != -1)
                 {
                     if (GetProcesses().TryGetValue(pid, out Process process))
                     {
                         if (IsWindowVisible(hwnd) && !blacklist.Contains(process.ProcessName))
                         {
                             return process;
                         }
                     }
                 }
                 return null;
             }

             private ConcurrentDictionary<int, Process> GetProcesses()
             {
                 return processes;
             }

             private Process? GetProcessById(int id)
             {
                 if (GetProcesses().TryGetValue(id, out Process process))
                 {
                     return process;
                 }
                 return null;
             }*/


            public Process? FilterProcesses(nint hwnd = default, int processID = default)
            {
                int pid = 0;
                if (processID > 0 && hwnd == default)
                {
                    pid = GetProcesses().Any(p => p.Id == processID) ? GetProcessById(processID).Id : -1;

                }
                else
                {
                    _ = GetWindowThreadProcessId(hwnd, out pid);
                    pid = GetProcesses().Any(p => p.Id == pid) ? pid : -1;
                }
                if (pid != -1)
                {
                    var process = GetProcessById(pid);
                    if (process != null)
                    {
                        if (!blacklist.Contains(process.ProcessName))
                        {
                            return process;
                        }
                    }
                }
                return null;
            }

        }
    }
}