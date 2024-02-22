using Frame.Exec;
using Frame.Helpers;
using Frame.Settings;
using static Frame.Helpers.LibImports;

namespace Frame.Windows;

public partial class WindowService
{
    public class Animator : IInit, IOnExit
    {
        public void Init()
        {
            Compositor.StartMove += ScheduleMove;
        }

        private void ScheduleMove(Window window)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                new Mover(AppSettings.AnimationType, window);
            });
        }


        /*
       * The animator manages the animation of the windows
       * It starts the animation.
       * It pauses it.
       * It sends a completion callback.
       *
       *
       *
       Need a check if a window was closed before the animation ends, need to handle such a case.
       */


        public void OnExit()
        {
            Compositor.StartMove -= ScheduleMove;
        }
    }

    public class Mover : LTick
    {
        private bool _isComplite;
        private AnimationType _animationType;
        private Window _window;


        public Mover(AnimationType animationType, Window window)
        {
            GC.KeepAlive(this);
            _animationType = animationType;
            _window = window;
            _isComplite = true;
        }

        private void Move(int tick)
        {
            IAnimation animation;
            if (_window.Transform.X == _window.TargetTransform.X && _window.Transform.Y == _window.TargetTransform.Y)
            {
                _isComplite = false;
                GC.SuppressFinalize(this);
            }
            
            switch (_animationType)
            {
                case AnimationType.LINEAR:
                    animation = new LinearAnimation();
                    break;
            }
            var la = new LinearAnimation();
            var temp = la.Animate(tick, _window.Transform, _window.TargetTransform);
            MoveWindow(_window.Hwnd, temp.X, temp.Y, temp.Width, temp.Height, true);
        }

        int _tick = 0;

        protected override void Tick()
        {
            if (!_isComplite) return;
            _tick += 10;
            Move(_tick);
            if (_tick >= 3000)
                GC.SuppressFinalize(this);
        }
    }
}