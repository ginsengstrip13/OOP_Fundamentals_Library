using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace lab6.Rendering
{
    public class VectorRenderer : IRenderer
    {
        public void RenderShape(string shapeName) =>
            Console.WriteLine($"[Векторный рендер]: Рисуем {shapeName} с помощью математических формул.");
    }
}