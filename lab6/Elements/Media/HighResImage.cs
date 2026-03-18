using System;
using System.Threading;

namespace lab6.Elements.Media
{
    public class HighResImage : IImage
    {
        private string _filePath;

        public HighResImage(string filePath)
        {
            _filePath = filePath;
            LoadImageFromDisk();
        }

        private void LoadImageFromDisk()
        {
            Console.WriteLine($"\n[HighResImage]: ДОЛГАЯ ЗАГРУЗКА файла {_filePath} (100 МБ) в память...");
            Thread.Sleep(1000); // Имитация долгой загрузки
        }

        public void Render() => Console.WriteLine($"[HighResImage]: Отрисовка изображения {_filePath}.");
    }
}