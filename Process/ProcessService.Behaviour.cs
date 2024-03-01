using Frame.Exec;

namespace Frame.ProcessSercive
{
    public partial class ProcessesSercive
    {
        public class Behaviour : IInit
        {
            private WinEvents _winEvents;
            private Trace _trace;

            public void Init()
            {
                _trace = new Trace();
                _winEvents = new WinEvents();
            }
        }
    }
}
