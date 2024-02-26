namespace Frame.Comands;

using static Helpers.LibImports;

public struct CloseWindow : ICommand<IntPtr>
{
    public void Invoke(IntPtr hwnd)
    {
        SendMessage(hwnd, (uint)WM.CLOSE, IntPtr.Zero, IntPtr.Zero);
    }
}