using System;
using System.Linq;

namespace lab5.Patterns.Composite
{
    public sealed class File : FileSystemItem
    {
        private byte[] _data;

        public File(string name, long size) : base(name)
        {
            if (size < 0 || size > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Размер файла должен помещаться в демонстрационный массив.");
            }

            _data = new byte[size];
        }

        public File(string name, byte[] data) : base(name)
        {
            _data = data.ToArray();
        }

        public override long GetSize()
        {
            return _data.LongLength;
        }

        public byte[] Read()
        {
            return _data.ToArray();
        }

        public void Write(byte[] data)
        {
            _data = data.ToArray();
        }

        public override FileSystemItem Copy()
        {
            return new File(Name, Read());
        }

        public override void Delete()
        {
            Console.WriteLine($"[Composite] Файл удален: {GetAbsolutePath()}");
            _data = [];
        }

        public override void Print(string indent = "")
        {
            Console.WriteLine($"{indent}- {Name} ({GetSize()} байт)");
        }
    }
}
