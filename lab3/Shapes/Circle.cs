using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab3
{
    public class Circle : IShape
    {
        public void Draw(Canvas canvas, Brush color, double x, double y)
        {
            Ellipse ellipse = new Ellipse { Width = 60, Height = 60, Fill = color };
            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
            canvas.Children.Add(ellipse);
        }
    }
}