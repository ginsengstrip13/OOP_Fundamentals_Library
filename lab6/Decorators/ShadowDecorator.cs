using System;
using lab6.Core;

namespace lab6.Decorators
{
    public class ShadowDecorator : GraphicDecorator
    {
        public ShadowDecorator(IDocumentItem item) : base(item) { }

        public override void Render()
        {
            base.Render();
            Console.WriteLine("   --> Добавлена реалистичная ТЕНЬ");
        }
    }
}