using System;

namespace lab6.Elements.Text
{
    public class TextStyle
    {
        public string FontName { get; }
        public int FontSize { get; }
        public string Color { get; }

        public TextStyle(string font, int size, string color)
        {
            FontName = font;
            FontSize = size;
            Color = color;
            Console.WriteLine($"[Flyweight]: Создан новый стиль: {font}, {size}pt, {color}");
        }
    }
}