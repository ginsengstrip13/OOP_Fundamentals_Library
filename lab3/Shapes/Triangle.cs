using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab3
{
    public class Triangle : IShape
    {
        public void Draw(Canvas canvas, Brush color, double x, double y)
        {
            Polygon triangle = new Polygon
            {
                Points = new PointCollection { new Point(30, 0), new Point(0, 60), new Point(60, 60) },
                Fill = color
            };
            Canvas.SetLeft(triangle, x);
            Canvas.SetTop(triangle, y);
            canvas.Children.Add(triangle);
        }
    }
}