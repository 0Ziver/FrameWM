using Frame.Helpers;
using static Frame.Helpers.LibImports;

namespace Frame.Commands
{
    public class MoveWindowToCenter : ICommand
    {
        private const int SW_RESTORE = 9;

        public void Execute()
        {
           /* int activeWindowHandle = ForegroundWindow.GetActiveWindowHandle();

            if (activeWindowHandle != IntPtr.Zero)
            {
                ShowWindow(activeWindowHandle, SW_RESTORE);

                int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                int screenHeight = Screen.PrimaryScreen.Bounds.Height;

                RECT windowRect;
                GetWindowRect(activeWindowHandle, out windowRect);

                int newX = (screenWidth - (windowRect.Right - windowRect.Left)) / 2;
                int newY = (screenHeight - (windowRect.Bottom - windowRect.Top)) / 2;

                SetWindowPos(activeWindowHandle, IntPtr.Zero, newX, newY, 0, 0, SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOZORDER);
            }*/
        }
    }
}
