namespace Frame.Windows;

public partial class WindowService
{
    public enum AnimationType
    {
        LINEAR
    }
    internal interface IAnimation
    {
        Transform Animate(float t, Transform start, Transform end);
    }

    public class LinearAnimation : IAnimation
    {
        public Transform Animate(float t, Transform start, Transform end)
        {
            return new Transform
            {
                X = (int)(start.X + t * (end.X - start.X)),
                Y = (int)(start.Y + t * (end.Y - start.Y)),
                Width = (int)(start.Width + t * (end.Width - start.Width)),
                Height = (int)(start.Height + t * (end.Height - start.Height))
            };
        }
    }

    #region To be finalized
    /*public class BezierAnimation : IAnimation
    {
        private readonly Transform _controlPoint1;
        private readonly Transform _controlPoint2;

        public BezierAnimation(Transform controlPoint1, Transform controlPoint2)
        {
            this._controlPoint1 = controlPoint1;
            this._controlPoint2 = controlPoint2;
        }

        public Transform Animate(float t, Transform start, Transform end)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            return new Transform
            {
                X = (int)(uuu * start.X + 3 * uu * t * _controlPoint1.X + 3 * u * tt * _controlPoint2.X + ttt * end.X),
                Y = (int)(uuu * start.Y + 3 * uu * t * _controlPoint1.Y + 3 * u * tt * _controlPoint2.Y + ttt * end.Y),
                Width = (int)(uuu * start.Width + 3 * uu * t * _controlPoint1.Width +
                              3 * u * tt * _controlPoint2.Width + ttt * end.Width),
                Height = (int)(uuu * start.Height + 3 * uu * t * _controlPoint1.Height +
                               3 * u * tt * _controlPoint2.Height + ttt * end.Height)
            };
        }
    }

    public class HermiteInterpolation : IAnimation
    {
        private readonly Transform _tangent1;
        private readonly Transform _tangent2;

        public HermiteInterpolation(Transform tangent1, Transform tangent2)
        {
            _tangent1 = tangent1;
            _tangent2 = tangent2;
        }

        public Transform Animate(float t, Transform start, Transform end)
        {
            float t2 = t * t;
            float t3 = t2 * t;

            return new Transform
            {
                X = (int)((2 * t3 - 3 * t2 + 1) * start.X +
                          (t3 - 2 * t2 + t) * _tangent1.X +
                          (-2 * t3 + 3 * t2) * end.X +
                          (t3 - t2) * _tangent2.X),
                Y = (int)((2 * t3 - 3 * t2 + 1) * start.Y +
                          (t3 - 2 * t2 + t) * _tangent1.Y +
                          (-2 * t3 + 3 * t2) * end.Y +
                          (t3 - t2) * _tangent2.Y),
                Width = (int)((2 * t3 - 3 * t2 + 1) * start.Width +
                              (t3 - 2 * t2 + t) * _tangent1.Width +
                              (-2 * t3 + 3 * t2) * end.Width +
                              (t3 - t2) * _tangent2.Width),
                Height = (int)((2 * t3 - 3 * t2 + 1) * start.Height +
                               (t3 - 2 * t2 + t) * _tangent1.Height +
                               (-2 * t3 + 3 * t2) * end.Height +
                               (t3 - t2) * _tangent2.Height)
            };
        }
    }*/
    #endregion
}
