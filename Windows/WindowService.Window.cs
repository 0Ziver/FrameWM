using static Frame.Helpers.LibImports;

namespace Frame.Windows
{
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
            nint hwnd,
            string name,
            int pid,
            WindowTransform transform)
        {
            public nint Hwnd { get; private set; } = hwnd;
            public string Name { get; private set; } = name;
            public int ProcessId { get; private set; } = pid;
            public WindowTransform Transform { get; private set; } = transform;
            public WindowTransform TargetTransform;
            

            public void Close()
            {
                SendMessage(Hwnd, (uint)WM.CLOSE, 0, 0);
            }

            public void Minimize()
            {
                SendMessage(Hwnd, (uint)WM.SYSCOMMAND, (nint)WM.SC_MINIMIZE, 0);
            }

            public void Restore()
            {
                SendMessage(Hwnd, (uint)WM.SYSCOMMAND, (nint)WM.SC_RESTORE, 0);
            }
        }
    }
}