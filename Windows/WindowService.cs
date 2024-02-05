using Frame.Exec;
using Frame.Helpers;
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
                        rect: tempRect,
                        width: tempRect.Right - tempRect.Left,
                        height: tempRect.Bottom - tempRect.Top));

            windows.Add(window);
            OnNewWindowReady?.Invoke(window);
        }
        private void Delete(Process process)
        {
            nint HWND = process.MainWindowHandle;
            windows.RemoveWhere(item => item.HWND == HWND);
        }

    }

    // Window Holder
    public partial class WindowService
    {
        public struct WindowTransform
        {
            public RECT rect;

            public int Width;
            public int Height;

            public WindowTransform(RECT rect, int width, int height)
            {
                this.rect = rect;
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
}