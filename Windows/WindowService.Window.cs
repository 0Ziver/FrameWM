namespace Frame.Windows
{
    public partial class WindowService
    {
        public struct Transform(int x, int y, int width, int height)
        {
            public int X { get; internal init; } = x;
            public int Y { get; internal init; } = y;
            public int Width { get; internal init; } = width;
            public int Height { get; internal init; } = height;
        }

        public struct Window(
            nint hwnd,
            string name,
            int pid,
            Transform transform)
        {
            public nint Hwnd { get; private set; } = hwnd;
            public string Name { get; private set; } = name;
            public int ProcessId { get; private set; } = pid;
            public Transform Transform { get; private set; } = transform;
            public Transform MinSize;
            public Transform TargetTransform;
        }
    }
}