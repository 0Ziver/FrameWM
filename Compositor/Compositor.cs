using Frame.Exec;

namespace Frame.Windows
{
    public class Compositor : IInit, IOnExit
    {
        public void Init()
        {
            WindowService.OnNewWindowReady += Compose;
        }

        private void Compose(WindowService.Window window)
        {
            window.Move(0, 0, 1280, 720);
        }



        private void CalculateNewRect()
        {

        }
        private void CalculateGap()
        {

        }




        public void OnExit()
        {
        }
    }
}
