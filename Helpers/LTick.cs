﻿using Timer = System.Threading.Timer;

namespace Frame.Helpers
{
    public abstract class LTick : IDisposable
    {
        Timer timer;
        public void Start()
        {
            timer = new Timer(Loop, null, Timeout.Infinite, Timeout.Infinite);
            timer?.Change(0, 16);
        }

        private void Stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void Loop(object? state)
        {
            Tick();
        }
        /// <summary>
        /// Invoke every 16ms
        /// </summary>
        protected abstract void Tick();

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
