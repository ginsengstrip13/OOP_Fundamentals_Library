namespace lab3
{
    public class SquareCreator : ShapeCreator
    {
        public override IShape CreateShape() => new Square();
    }
}