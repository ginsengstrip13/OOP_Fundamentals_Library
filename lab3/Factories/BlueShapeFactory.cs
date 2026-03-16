using System.Windows.Media;

namespace lab3
{
    public class BlueShapeFactory : IShapeFactory
    {
        public IShape CreateCircle() => new Circle(Brushes.Blue);
        public IShape CreateSquare() => new Square(Brushes.Blue);
        public IShape CreateTriangle() => new Triangle(Brushes.Blue);
    }
}