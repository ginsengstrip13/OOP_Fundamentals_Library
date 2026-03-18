using System.Collections.Generic;

namespace lab6.Elements.Text
{
    public class TextStyleFactory
    {
        private Dictionary<string, TextStyle> _styles = new Dictionary<string, TextStyle>();

        public TextStyle GetStyle(string font, int size, string color)
        {
            string key = $"{font}_{size}_{color}";
            if (!_styles.ContainsKey(key))
            {
                _styles[key] = new TextStyle(font, size, color);
            }
            return _styles[key];
        }
    }
}