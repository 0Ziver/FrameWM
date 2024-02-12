using Frame.Exec;
using System.Diagnostics;
using static Frame.Helpers.LibImports;

namespace Frame.Windows
{
    public partial class WindowService : IInit
    {
        public static event Action<Window> OnNewWindowReady;

        private HashSet<Window> windows = new(128);

        public void Init()
        {
            // ProcessTraceService.OnProcessStart += Cast;
            //  ProcessTraceService.OnProcessStop += Delete;
        }

        private void Cast(Process process)
        {
            RECT tempRect;
            GetWindowRect(process.MainWindowHandle, out tempRect);
            var window = new Window(
                HWND: process.MainWindowHandle,
                NAME: process.ProcessName,
                PID: process.Id,
                Transform: new WindowTransform(
                    /*
                     * Fill wT
                     * 
                     */
                    ));

            windows.Add(window);
            OnNewWindowReady?.Invoke(window);
        }

        private void Delete(Process process)
        {
            nint HWND = process.MainWindowHandle;
            windows.RemoveWhere(item => item.HWND == HWND);
        }
    }

    public partial class WindowService
    {
        public struct WindowTransform
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;

            public WindowTransform(int x, int y, int width, int height)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }
        }

        public struct Window(
            nint HWND,
            string NAME,
            int PID,
            WindowTransform Transform)
        {
            public nint HWND { get; private set; } = HWND;
            public string NAME { get; private set; } = NAME;
            public int PID { get; private set; } = PID;
            public WindowTransform Transform { get; private set; } = Transform;

            public void Move(int newX, int newY, int newWidth, int newHeight)
            {
                Restore();
                MoveWindow(
                    HWND,
                    newX,
                    newY,
                    newWidth,
                    newHeight,
                    true);
            }

            public void Close()
            {
                SendMessage(HWND, (uint)WM.CLOSE, 0, 0);
            }

            public void Minimize()
            {
                SendMessage(HWND, (uint)WM.SYSCOMMAND, (nint)WM.SC_MINIMIZE, 0);
            }

            public void Restore()
            {
                SendMessage(HWND, (uint)WM.SYSCOMMAND, (nint)WM.SC_RESTORE, 0);
            }
        }
    }

    public partial class WindowService
    {
        internal interface IAnimator
        {
            WindowTransform Animate(float t, WindowTransform start, WindowTransform end);
        }

        public class LinearAnimation : IAnimator
        {
            public WindowTransform Animate(float t, WindowTransform start, WindowTransform end)
            {
                return new WindowTransform
                {
                    X = (int)(start.X + t * (end.X - start.X)),
                    Y = (int)(start.Y + t * (end.Y - start.Y)),
                    Width = (int)(start.Width + t * (end.Width - start.Width)),
                    Height = (int)(start.Height + t * (end.Height - start.Height))
                };
            }
        }

        public class BezierAnimation : IAnimator
        {
            private readonly WindowTransform _controlPoint1;
            private readonly WindowTransform _controlPoint2;

            public BezierAnimation(WindowTransform controlPoint1, WindowTransform controlPoint2)
            {
                this._controlPoint1 = controlPoint1;
                this._controlPoint2 = controlPoint2;
            }

            public WindowTransform Animate(float t, WindowTransform start, WindowTransform end)
            {
                float u = 1 - t;
                float tt = t * t;
                float uu = u * u;
                float uuu = uu * u;
                float ttt = tt * t;

                return new WindowTransform
                {
                    X = (int)(uuu * start.X + 3 * uu * t * _controlPoint1.X + 3 * u * tt * _controlPoint2.X + ttt * end.X),
                    Y = (int)(uuu * start.Y + 3 * uu * t * _controlPoint1.Y + 3 * u * tt * _controlPoint2.Y + ttt * end.Y),
                    Width = (int)(uuu * start.Width + 3 * uu * t * _controlPoint1.Width + 3 * u * tt * _controlPoint2.Width + ttt * end.Width),
                    Height = (int)(uuu * start.Height + 3 * uu * t * _controlPoint1.Height + 3 * u * tt * _controlPoint2.Height + ttt * end.Height)
                };
            }
        }

        public class Animator
        {
            public void Start()
            {
                ThreadPool.QueueUserWorkItem(_ => { });
            }

            /*private void I9()
            {
                new LinearAnimation().Animate()
            }*/
        }
    }
}