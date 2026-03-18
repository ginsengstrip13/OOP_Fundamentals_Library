using lab6.Core;

namespace lab6.Decorators
{
    public abstract class GraphicDecorator : IDocumentItem
    {
        protected IDocumentItem _wrappee;

        public GraphicDecorator(IDocumentItem item)
        {
            _wrappee = item;
        }

        public virtual void Render()
        {
            _wrappee.Render();
        }
    }
}