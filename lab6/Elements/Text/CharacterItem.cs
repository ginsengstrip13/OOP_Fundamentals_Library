using System;
using lab6.Core;

namespace lab6.Elements.Text
{
    public class CharacterItem : IDocumentItem
    {
        private char _symbol;
        private TextStyle _style;

        public CharacterItem(char symbol, TextStyle style)
        {
            _symbol = symbol;
            _style = style;
        }

        public void Render() =>
            Console.WriteLine($"[Текст]: Рисуем '{_symbol}' стилем ({_style.FontName}, {_style.FontSize}pt, {_style.Color})");
    }
}