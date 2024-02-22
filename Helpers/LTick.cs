using Timer = System.Threading.Timer;

namespace Frame.Helpers
{
    public abstract class LTick : IDisposable
    {
        Timer _timer;
        public void Start()
        {
            _timer = new Timer(Loop, null, Timeout.Infinite, Timeout.Infinite);
            _timer?.Change(0, 10);
        }

        private void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void Loop(object? state)
        {
            Tick();
        }

        /// <summary>
        /// Invoke every 10ms
        /// </summary>
        /// <param name="isReady"></param>
        protected abstract void Tick();

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
