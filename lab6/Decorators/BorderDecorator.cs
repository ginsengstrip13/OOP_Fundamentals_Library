using System;
using lab6.Core;

namespace lab6.Decorators
{
    public class BorderDecorator : GraphicDecorator
    {
        public BorderDecorator(IDocumentItem item) : base(item) { }

        public override void Render()
        {
            base.Render();
            Console.WriteLine("   --> Добавлена красивая цветная РАМКА");
        }
    }
}