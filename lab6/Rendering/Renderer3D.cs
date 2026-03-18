using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace lab6.Rendering
{
    public class Renderer3D : IRenderer
    {
        public void RenderShape(string shapeName) =>
            Console.WriteLine($"[3D рендер]: Генерируем полигоны для {shapeName} в трехмерном пространстве.");
    }
}