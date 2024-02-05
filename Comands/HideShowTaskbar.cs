using Frame.Helpers;

namespace Frame.Commands
{
    public class HideShowTaskbar : ICommand
    {
        private const string CLASSNAME = "Shell_TrayWnd";
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private bool isActive = true;
        private readonly string? _;
        private int HWND;

        public HideShowTaskbar()
        {
            HWND = LibImports.FindWindow(CLASSNAME, _);
        }

        public void Execute()
        {
            isActive = !isActive;
            LibImports.ShowWindow(HWND, isActive ? SW_SHOW : SW_HIDE);
        }

        ~HideShowTaskbar()
        {
            LibImports.ShowWindow(HWND, SW_SHOW);
        }
    }
}
