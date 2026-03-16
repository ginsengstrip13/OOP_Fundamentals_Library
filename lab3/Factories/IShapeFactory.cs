namespace lab3
{
    public interface IShapeFactory
    {
        IShape CreateCircle();
        IShape CreateSquare();
        IShape CreateTriangle();
    }
}