using Frame.Exec;

namespace Frame.Helpers
{
    internal class Initializer : IInit
    {
        public void Init()
        {
            Console.WriteLine($@"{typeof(Initializer)} is ready");
        }
    }
}
