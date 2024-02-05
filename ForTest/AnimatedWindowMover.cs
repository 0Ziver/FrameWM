/*using Frame.Helpers;
using static Frame.Helpers.LibImports;
using static Frame.Windows.WindowService;
public interface IAnimation
{
    WindowTransform Animate(float t, WindowTransform start, WindowTransform end);
}


public class LinearAnimation : IAnimation
{
    public WindowTransform Animate(float t, WindowTransform start, WindowTransform end)
    {
        return new WindowTransform
        {
            X = (int)(start.X + t * (end.X - start.X)),
            Y = (int)(start.Y + t * (end.Y - start.Y)),
            Width = (int)(start.Width + t * (end.Width - start.Width)),
            Height = (int)(start.Height + t * (end.Height - start.Height))
        };
    }
}
public class QuadraticAnimation : IAnimation
{
    public WindowTransform Animate(float t, WindowTransform start, WindowTransform end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return new WindowTransform
        {
            X = (int)(uu * start.X + 2 * u * t * start.X + tt * end.X),
            Y = (int)(uu * start.Y + 2 * u * t * start.Y + tt * end.Y),
            Width = (int)(uu * start.Width + 2 * u * t * start.Width + tt * end.Width),
            Height = (int)(uu * start.Height + 2 * u * t * start.Height + tt * end.Height)
        };
    }
}
public class CubicBezierAnimation : IAnimation
{
    private WindowTransform controlPoint1;
    private WindowTransform controlPoint2;

    public CubicBezierAnimation(WindowTransform controlPoint1, WindowTransform controlPoint2)
    {
        this.controlPoint1 = controlPoint1;
        this.controlPoint2 = controlPoint2;
    }

    public WindowTransform Animate(float t, WindowTransform start, WindowTransform end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        return new WindowTransform
        {
            X = (int)(uuu * start.X + 3 * uu * t * controlPoint1.X + 3 * u * tt * controlPoint2.X + ttt * end.X),
            Y = (int)(uuu * start.Y + 3 * uu * t * controlPoint1.Y + 3 * u * tt * controlPoint2.Y + ttt * end.Y),
            Width = (int)(uuu * start.Width + 3 * uu * t * controlPoint1.Width + 3 * u * tt * controlPoint2.Width + ttt * end.Width),
            Height = (int)(uuu * start.Height + 3 * uu * t * controlPoint1.Height + 3 * u * tt * controlPoint2.Height + ttt * end.Height)
        };
    }
}
public class HermiteInterpolation : IAnimation
{
    private WindowTransform tangent1;
    private WindowTransform tangent2;

    public HermiteInterpolation(WindowTransform tangent1, WindowTransform tangent2)
    {
        this.tangent1 = tangent1;
        this.tangent2 = tangent2;
    }

    public WindowTransform Animate(float t, WindowTransform start, WindowTransform end)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return new WindowTransform
        {
            X = (int)((2 * t3 - 3 * t2 + 1) * start.X +
                      (t3 - 2 * t2 + t) * tangent1.X +
                      (-2 * t3 + 3 * t2) * end.X +
                      (t3 - t2) * tangent2.X),
            Y = (int)((2 * t3 - 3 * t2 + 1) * start.Y +
                      (t3 - 2 * t2 + t) * tangent1.Y +
                      (-2 * t3 + 3 * t2) * end.Y +
                      (t3 - t2) * tangent2.Y),
            Width = (int)((2 * t3 - 3 * t2 + 1) * start.Width +
                           (t3 - 2 * t2 + t) * tangent1.Width +
                           (-2 * t3 + 3 * t2) * end.Width +
                           (t3 - t2) * tangent2.Width),
            Height = (int)((2 * t3 - 3 * t2 + 1) * start.Height +
                            (t3 - 2 * t2 + t) * tangent1.Height +
                            (-2 * t3 + 3 * t2) * end.Height +
                            (t3 - t2) * tangent2.Height)
        };
    }
}



public class Animator
{
    public void Animate()
    {
        *//*
         * Когда открывается новое окно WindowService оборачивает его в класс Window
         * - и присылает ивент в Compositor
         * Compositor отправляет ивент сюда, о том, что нужно передвинуть окно и передает необходимый RECT.
         * В Animator по ивенту в отдельном потоке из ThreadPool запускается короткий цикл, который 
         *  - высчитывает анимацию и при каждом шаге дергает Move(){} класс Window. Получается плавная анимация
         *  Когда Compositor присылает ивент, нужно прислать FPS. Что бы Animator обработал все с нужной частотой
         *//*
    }
}



*/