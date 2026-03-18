using System;

namespace lab6.Elements.Media
{
    public class ImageProxy : IImage
    {
        private HighResImage _realImage;
        private string _filePath;

        public ImageProxy(string filePath)
        {
            _filePath = filePath;
            Console.WriteLine($"[Proxy]: Создан прокси для {_filePath}. Картинка пока НЕ загружена.");
        }

        public void Render()
        {
            if (_realImage == null)
            {
                _realImage = new HighResImage(_filePath);
            }
            _realImage.Render();
        }
    }
}