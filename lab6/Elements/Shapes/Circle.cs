using lab6.Rendering;

namespace lab6.Elements.Shapes
{
    public class Circle : Shape
    {
        public Circle(IRenderer renderer) : base(renderer) { }
        
        public override void Render() => _renderer.RenderShape("Круг");
    }
}