using System;

namespace lab5.Patterns.Composite
{
    public abstract class FileSystemItem
    {
        protected FileSystemItem(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public Folder? Parent { get; internal set; }

        public abstract long GetSize();

        public virtual void Add(FileSystemItem item)
        {
            throw new InvalidOperationException("Файл не может содержать вложенные элементы.");
        }

        public virtual void Remove(FileSystemItem item)
        {
            throw new InvalidOperationException("Файл не может содержать вложенные элементы.");
        }

        public virtual FileSystemItem? GetChild(int index)
        {
            throw new InvalidOperationException("Файл не может содержать вложенные элементы.");
        }

        public abstract FileSystemItem Copy();

        public abstract void Delete();

        public abstract void Print(string indent = "");

        public string GetAbsolutePath()
        {
            return Parent is null
                ? "/" + Name
                : Parent.GetAbsolutePath() + "/" + Name;
        }
    }
}
