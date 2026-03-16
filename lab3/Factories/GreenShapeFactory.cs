using System.Windows.Media;

namespace lab3
{
    public class GreenShapeFactory : IShapeFactory
    {
        public IShape CreateCircle() => new Circle(Brushes.Green);
        public IShape CreateSquare() => new Square(Brushes.Green);
        public IShape CreateTriangle() => new Triangle(Brushes.Green);
    }
}