using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5.Patterns.Composite
{
    // "Компоновщик" - папка (может содержать файлы и другие папки)
    public class DirectoryItem : FileSystemNode
    {
        private List<FileSystemNode> _children = new List<FileSystemNode>();

        public DirectoryItem(string name) : base(name) { }

        public void Add(FileSystemNode node) => _children.Add(node);
        public void Remove(FileSystemNode node) => _children.Remove(node);

        public override int GetSize() => _children.Sum(child => child.GetSize());

        public override void Delete()
        {
            foreach (var child in _children.ToList())
            {
                child.Delete();
            }
            Console.WriteLine($"[Папка] Удалена: {Name}");
        }

        public override FileSystemNode Copy()
        {
            var newDir = new DirectoryItem(Name + "_Copy");
            foreach (var child in _children)
            {
                newDir.Add(child.Copy());
            }
            return newDir;
        }

        public override void Print(string indent = "")
        {
            Console.WriteLine($"{indent}+ {Name} (Размер: {GetSize()} байт)");
            foreach (var child in _children)
            {
                child.Print(indent + "  ");
            }
        }
    }
}