using Frame.Comands;

namespace Frame.Commands
{
    public class CommandHolder
    {
        public static CloseWindows CW;
        public static HideShowTaskbar HST;
        public static MoveWindowToBack MWTB;
        public static MoveWindowToCenter MWTC;
        public static MoveWindowToNext MWTN;
        public static SwitchLayout SL;
        public static ThemeSwitcher TS;

        public CommandHolder()
        {
            CW = new CloseWindows();
            HST = new HideShowTaskbar();
            MWTB = new MoveWindowToBack();
            MWTC = new MoveWindowToCenter();
            MWTN = new MoveWindowToNext();
            SL = new SwitchLayout();
            TS = new ThemeSwitcher();
        }
    }
}