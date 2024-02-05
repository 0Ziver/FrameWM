using Frame.Configs;
using Frame.Exec;

namespace Frame.Helpers
{
    internal class Initializer : IInit
    {
        public static Config config;
        public static ScreenInfo screenInfo;




        public Initializer()
        {
            screenInfo = new();
        }

        public void Init()
        {
        }
    }
}
