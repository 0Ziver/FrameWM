using Frame.Input;

namespace Frame.Configs
{
    internal struct Config
    {
        public ScreenInfo SYS_INFO;

        public Config()
        {
        }
    }


    internal enum Layout
    {
        GRID,
        FLEX,
        ROW,
        COLLUM
    }
    internal struct FrameSettings
    {
        public int RESIZE_STEP_HORIZONTAL;
        public int RESIZE_STEP_VERTICAL;
        public int PADDING;
        public Layout LAYOUT;
        public KeyCode MOD_KEY;
        public KeyCode SEC_KEY;

        public FrameSettings()
        {

        }
    }


    internal struct ScreenInfo
    {
        public Dictionary<int, ResolutionInfo> DisplayResolutions;

        public ScreenInfo()
        {
            DisplayResolutions = new Dictionary<int, ResolutionInfo>();
            Screen[] screens = Screen.AllScreens;

            for (int i = 0; i < screens.Length; i++)
            {
                Screen screen = screens[i];
                int screenWidth = screen.Bounds.Width;
                int screenHeight = screen.Bounds.Height;

                DisplayResolutions.Add(i, new ResolutionInfo(screenWidth, screenHeight));
            }

            DisplayCount = screens.Length;
        }

        public int DisplayCount { get; private set; }
    }

    internal struct ResolutionInfo
    {
        public int Width;
        public int Height;

        public ResolutionInfo(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    internal struct SystemShortcutBlocklist
    {
        public Hotkey[] Blocklist;


        public SystemShortcutBlocklist()
        {
            Blocklist =
            [
            new Hotkey(f: KeyCode.LWIN, k: KeyCode.KEY_P),
                new Hotkey(f: KeyCode.LWIN, k: KeyCode.KEY_U)
            ];
        }

        public Hotkey GetBlocklist()
        {
            foreach (var blockShortcut in Blocklist)
            {
                return blockShortcut;
            }
            return new(0, 0, 0);
        }
    }
}
