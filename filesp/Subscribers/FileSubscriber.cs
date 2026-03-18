using System;
using System.IO;
using filesp.Strategies;

namespace filesp.Subscribers
{
    public class FileSubscriber : BaseSubscriber
    {
        private string _filePath;

        public FileSubscriber(IFormatStrategy formatStrategy, string filePath) : base(formatStrategy)
        {
            _filePath = filePath;
        }

        protected override void DeliverMessage(string message)
        {
            File.AppendAllText(_filePath, message + Environment.NewLine);
            Console.WriteLine($"[File] => Записано в файл {_filePath}");
        }
    }
}