using Frame.Input;

namespace Frame.Configs
{
    public struct Hotkey(KeyCode f = 0, KeyCode s = 0, KeyCode k = 0)
    {
        public KeyCode ModFirst = f;
        public KeyCode ModSecond = s;
        public KeyCode Key = k;
    }
}
