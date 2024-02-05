using Frame.Helpers;

namespace Frame.Commands
{
    public class CloseWindows : ICommand<nint>
    {
        private const int WM_CLOSE = 0x0010;

        public void Execute(nint arg)
        {
            LibImports.SendMessage(arg, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
