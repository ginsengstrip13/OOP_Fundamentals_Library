namespace lab3
{
    public class TriangleCreator : ShapeCreator
    {
        public override IShape CreateShape() => new Triangle();
    }
}