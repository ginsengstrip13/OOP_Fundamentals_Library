using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab3
{
    public class Triangle : IShape
    {
        private readonly Brush _color;

        public Triangle(Brush color)
        {
            _color = color;
        }

        public void Draw(Canvas canvas, double x, double y)
        {
            Polygon triangle = new Polygon
            {
                Points = new PointCollection { new Point(30, 0), new Point(0, 60), new Point(60, 60) },
                Fill = _color
            };
            Canvas.SetLeft(triangle, x);
            Canvas.SetTop(triangle, y);
            canvas.Children.Add(triangle);
        }
    }
}