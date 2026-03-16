namespace lab3
{
    public class CircleCreator : ShapeCreator
    {
        public override IShape CreateShape() => new Circle();
    }
}