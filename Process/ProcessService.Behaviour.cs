using Frame.Exec;

namespace Frame.Process
{
    public partial class ProcessService
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
