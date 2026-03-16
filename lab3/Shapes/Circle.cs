using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab3
{
    public class Circle : IShape
    {
        private readonly Brush _color;

        public Circle(Brush color)
        {
            _color = color;
        }

        public void Draw(Canvas canvas, double x, double y)
        {
            Ellipse ellipse = new Ellipse { Width = 60, Height = 60, Fill = _color };
            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
            canvas.Children.Add(ellipse);
        }
    }
}