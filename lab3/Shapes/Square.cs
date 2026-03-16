using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab3
{
    public class Square : IShape
    {
        private readonly Brush _color;

        public Square(Brush color)
        {
            _color = color;
        }

        public void Draw(Canvas canvas, double x, double y)
        {
            Rectangle rect = new Rectangle { Width = 60, Height = 60, Fill = _color };
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            canvas.Children.Add(rect);
        }
    }
}