using System.Diagnostics;
using System.Numerics;
using System.Text;
using Frame.Exec;
using Frame.Helpers;
using Frame.ProcessSercive;

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


            ProcessesSercive.Trace.OnProcessClose += process =>
            {
                Console.WriteLine($"{process.ProcessName} is closed");
            };
            ProcessesSercive.WinEvents.OnChangeFocus += pr =>
            {
                if (pr == null) return;
                Console.WriteLine($"{pr.ProcessName} in Focus");
            };
            ProcessesSercive.WinEvents.OnWindowMinimized += minz =>
            {
                if (minz == null) return;
                Console.WriteLine($"{minz.ProcessName} is Minimized");
            };
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

            var temp = new WindowService.Transform(y: 0,
                width: 800,
                height: 600, x: 0);

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
            /*
             * Вычести от прилегающей к другому окну стороный указанный gap
             * У остальных сторон вычесть padding
             */
            return 0;
        }


        public void OnExit()
        {
        }
    }
}