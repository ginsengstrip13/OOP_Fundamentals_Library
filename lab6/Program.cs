using System;
using lab6.Core;
using lab6.Rendering;
using lab6.Elements.Shapes;
using lab6.Elements.Media;
using lab6.Elements.Text;
using lab6.Decorators;

namespace lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 1. Тестируем BRIDGE (Фигуры и Рендеры) ===");
            IRenderer vectorEngine = new VectorRenderer();
            IRenderer engine3D = new Renderer3D();

            IDocumentItem circle2D = new Circle(vectorEngine);
            IDocumentItem circle3D = new Circle(engine3D);
            circle2D.Render();
            circle3D.Render();

            Console.WriteLine("\n=== 2. Тестируем PROXY (Отложенная загрузка) ===");
            IDocumentItem proxyImage = new ImageProxy("C:/images/universe_8k.png");
            Console.WriteLine("Документ открыт. Пользователь листает вниз...");
            proxyImage.Render(); // Грузится тут
            Console.WriteLine("Пользователь снова смотрит на картинку:");
            proxyImage.Render(); // Берется из кэша

            Console.WriteLine("\n=== 3. Тестируем FLYWEIGHT (Текст и Стили) ===");
            TextStyleFactory styleFactory = new TextStyleFactory();

            TextStyle boldRed = styleFactory.GetStyle("Arial", 14, "Red");
            TextStyle boldRedAgain = styleFactory.GetStyle("Arial", 14, "Red");

            IDocumentItem char1 = new CharacterItem('H', boldRed);
            IDocumentItem char2 = new CharacterItem('i', boldRedAgain);
            char1.Render();
            char2.Render();

            Console.WriteLine("\n=== 4. Тестируем DECORATOR (Визуальные эффекты) ===");
            IDocumentItem fancyCircle = new ShadowDecorator(new BorderDecorator(circle3D));
            fancyCircle.Render();

            Console.WriteLine("\n=== 5. МЕГА-КОМБО ===");
            IDocumentItem ultimateItem = new ShadowDecorator(
                                            new BorderDecorator(
                                                new ImageProxy("photo.jpg")));
            ultimateItem.Render();

            Console.ReadLine(); // Чтобы консоль не закрылась сразу
        }
    }
}