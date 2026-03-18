using lab6.Core;
using lab6.Rendering;

namespace lab6.Elements.Shapes
{
    public abstract class Shape : IDocumentItem
    {
        protected IRenderer _renderer;

        protected Shape(IRenderer renderer)
        {
            _renderer = renderer;
        }

        public abstract void Render();
    }
}