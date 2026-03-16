using System.Windows.Media;

namespace lab3
{
    public class RedShapeFactory : IShapeFactory
    {
        public IShape CreateCircle() => new Circle(Brushes.Red);
        public IShape CreateSquare() => new Square(Brushes.Red);
        public IShape CreateTriangle() => new Triangle(Brushes.Red);
    }
}