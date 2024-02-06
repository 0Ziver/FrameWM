using Frame.Commands;
using Frame.Configs;
using Frame.Exec;
using Frame.Helpers;
using Frame.Input;
using Frame.Windows;

namespace Frame
{
    internal class Startup : IDisposable
    {
        private const string iconPATH = "C:\\Users\\0_ziv\\RiderProjects\\FrameWM\\icons\\Tray.ico";
        
        private static NotifyIcon notifyIcon;
        private static Executor executor;

        static void Main()
        {
            new CommandHolder();
            //new ProcessStateHook();
            var a = new AliBaba();

            InitializeNotifyIcon();
            executor = new Executor();
            executor.Add(
                new Initializer(),
                new WindowService(),
                new ProcessTraceService(),
                new Compositor(),
                new Keyboard(),
                new Interpreter());
            executor.Initialization();
            Application.Run();
        }

        #region Tray

        static void InitializeNotifyIcon()
        {
            // 
            ContextMenuStrip context = new();
            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "Frame";
            notifyIcon.Icon = new Icon(iconPATH);
            notifyIcon.Visible = true;
            notifyIcon.ContextMenuStrip = context;


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
            notifyIcon.Dispose();
            executor.OnExit();
            Application.Exit();
        }

        public void Dispose()
        {
            notifyIcon.Dispose();
            executor.OnExit();
        }

        #endregion
    }
}