using System;

namespace lab5.Patterns.Composite
{
    
    public class FileItem : FileSystemNode
    {
        private int _size;

        public FileItem(string name, int size) : base(name)
        {
            _size = size;
        }

        public override int GetSize() => _size;

        public override void Delete() => Console.WriteLine($"[Файл] Удален: {Name}");

        public override FileSystemNode Copy() => new FileItem(Name, _size);

        public override void Print(string indent = "") => Console.WriteLine($"{indent}- {Name} ({_size} байт)");
    }
}