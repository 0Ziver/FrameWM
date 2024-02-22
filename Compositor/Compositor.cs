using System.Numerics;
using System.Text;
using Frame.Exec;
using Frame.Helpers;

namespace Frame.Windows
{
    public class Compositor : IInit, IOnExit
    {
        private const string filePath = "Log.txt";

        struct Vec2Int
        {
            public int x;
            public int y;

            public Vec2Int(int y, int x)
            {
                this.y = y;
                this.x = x;
            }
        }

        public static event Action<WindowService.Window> StartMove;

        public void Init()
        {
            Console.WriteLine($@"{typeof(Compositor)} is ready");

            Process.ProcessService.ProcessChangeState.OnChangeFocus += process =>
            {
                if (process != null)
                {
                    LibImports.GetWindowThreadProcessId(process.MainWindowHandle, out uint lpdwProcessId);
                    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById((int)lpdwProcessId);
                    if (p.ProcessName.Length > 0)
                        WriteLog($"Changed focus on:{p.ProcessName}");
                    // Console.WriteLine($"Focus on: {p.ProcessName}");
                }
            };

            Process.ProcessService.ProcessTrace.PStart.OnNewProcessName += processName =>
            {
                WriteLog($"New Process: {processName}");
                // Console.WriteLine(processName);
            };

            /*Process.ProcessService.ProcessTrace.PStart.OnNewProcessCreated += process =>
            {
                if (process != null)
                {
                    LibImports.GetWindowThreadProcessId(process.MainWindowHandle, out uint lpdwProcessId);
                    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById((int)lpdwProcessId);
                    if (p.ProcessName.Length > 0)
                        Console.WriteLine($"Create: {p.ProcessName}");
                }
            };*/
        }

        void WriteLog(string line)
        {
            File.WriteAllLines(filePath, new[] { line });
        }


        private void Compose(WindowService.Window window)
        {
            /*
            var temp = new WindowService.WindowTransform(
                x: CalculateNewPosition().x,
                y: CalculateNewPosition().y,
                width: CalculateNewSize().x,
                height: CalculateNewSize().y);
                */

            var temp = new WindowService.WindowTransform(
                x: 0,
                y: 0,
                width: 800,
                height: 600);

            window.TargetTransform = temp;

            StartMove?.Invoke(window);
        }


        private Vec2Int CalculateNewSize()
        {
            return new Vec2Int(0, 0);
        }

        private Vec2Int CalculateNewPosition()
        {
            return new Vec2Int(0, 0);
        }

        private int CalculateGap()
        {
            return 0;
        }


        public void OnExit()
        {
        }
    }
}