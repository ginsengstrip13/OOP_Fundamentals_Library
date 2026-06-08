using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5.Patterns.Composite
{
    public sealed class Folder : FileSystemItem
    {
        private readonly List<FileSystemItem> _children = [];

        public Folder(string name) : base(name)
        {
        }

        public IReadOnlyList<FileSystemItem> Children => _children;

        public override long GetSize()
        {
            return _children.Sum(child => child.GetSize());
        }

        public override void Add(FileSystemItem item)
        {
            if (_children.Any(child => child.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"В папке '{Name}' уже есть элемент '{item.Name}'.");
            }

            item.Parent = this;
            _children.Add(item);
        }

        public override void Remove(FileSystemItem item)
        {
            if (_children.Remove(item))
            {
                item.Parent = null;
            }
        }

        public override FileSystemItem? GetChild(int index)
        {
            return index >= 0 && index < _children.Count ? _children[index] : null;
        }

        public FileSystemItem? FindChild(string name)
        {
            return _children.FirstOrDefault(child => child.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public override FileSystemItem Copy()
        {
            var copy = new Folder(Name);
            foreach (var child in _children)
            {
                copy.Add(child.Copy());
            }

            return copy;
        }

        public override void Delete()
        {
            foreach (var child in _children.ToList())
            {
                child.Delete();
                Remove(child);
            }

            Console.WriteLine($"[Composite] Папка удалена: {GetAbsolutePath()}");
        }

        public override void Print(string indent = "")
        {
            Console.WriteLine($"{indent}+ {Name} ({GetSize()} байт)");
            foreach (var child in _children)
            {
                child.Print(indent + "  ");
            }
        }
    }
}
