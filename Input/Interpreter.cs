using Frame.Exec;

namespace Frame.Input
{
    internal class Interpreter : IInit
    {
        
        public void Init()
        {
            Console.WriteLine($@"{typeof(Interpreter)} is ready");

        }
    }
}
