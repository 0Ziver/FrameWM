using Frame.Exec;
using Frame.Helpers;
using Frame.Input;
using Frame.Process;
using Frame.Time;
using Frame.Windows;
using System.Runtime.InteropServices;

namespace Frame
{
    internal class Startup : IDisposable
    {
        private const string IconPath = "C:\\Users\\0_ziv\\RiderProjects\\FrameWM\\icons\\Tray.ico";

        private static NotifyIcon _notifyIcon;
        private static Executor _executor;
        static void Main()
        {

            InitializeNotifyIcon();
            _executor = new Executor();
            _executor.Add(
                executors:
                [
                    new Initializer(),
                    new ProcessService.Behaviour(),
                    new Compositor(),
                    new Keyboard(),
                    new Interpreter()
                ]);
            _executor.Initialization();
            Application.Run();
        }


        static int fortest()
        {
            new LocalTime();
            return 0;
        }

        #region Tray

        static void InitializeNotifyIcon()
        {
            // 
            ContextMenuStrip context = new();
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "Frame";
            //   _notifyIcon.Icon = new Icon(IconPath);
            _notifyIcon.Visible = true;
            _notifyIcon.ContextMenuStrip = context;


            // 
            ToolStripMenuItem settingsItem = new ToolStripMenuItem("Settings");
            settingsItem.Click += OnAdditionalItemClick;
            context.Items.Add(settingsItem);

            // 
            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += OnExit;
            context.Items.Add(exitItem);
        }

        private static void OnAdditionalItemClick(object? sender, EventArgs e)
        {
            Form settings = new Frame.Forms.Settigns();
            settings.Show();
        }

        private static void OnExit(object? sender, EventArgs e)
        {
            _notifyIcon.Dispose();
            _executor.OnExit();
            Application.Exit();
        }

        public void Dispose()
        {
            _notifyIcon.Dispose();
            _executor.OnExit();
        }

        #endregion
    }
}