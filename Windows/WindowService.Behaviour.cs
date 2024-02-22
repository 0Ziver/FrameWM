using static Frame.Helpers.LibImports;

namespace Frame.Windows
{
    public partial class WindowService
    {
        public class WindowsBehaviour
        {
            public static event Action<Window> OnNewWindowReady;

            private readonly HashSet<Window> _windows = new(256);

            public void Init()
            {
            }


            private void Cast(System.Diagnostics.Process process)
            {
                GetWindowRect(process.MainWindowHandle, out var tempRect);
                var window = new Window(
                    hwnd: process.MainWindowHandle,
                    name: process.ProcessName,
                    pid: process.Id,
                    transform: new WindowTransform(
                        x: tempRect.Left,
                        y: tempRect.Top,
                        width: tempRect.Right - tempRect.Left,
                        height: tempRect.Bottom - tempRect.Top));

                _windows.Add(window);
                OnNewWindowReady(window);
            }

            private void Delete(System.Diagnostics.Process process)
            {
                nint hwnd = process.MainWindowHandle;
                _windows.RemoveWhere(item => item.Hwnd == hwnd);
            }
        }
    }
}