using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab3
{
    public class Square : IShape
    {
        public void Draw(Canvas canvas, Brush color, double x, double y)
        {
            Rectangle rect = new Rectangle { Width = 60, Height = 60, Fill = color };
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            canvas.Children.Add(rect);
        }
    }
}