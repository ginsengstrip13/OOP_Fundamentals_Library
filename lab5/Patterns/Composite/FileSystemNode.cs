using System;

namespace lab5.Patterns.Composite
{
    // Общий абстрактный класс для файлов и папок
    public abstract class FileSystemNode
    {
        public string Name { get; set; }

        public FileSystemNode(string name)
        {
            Name = name;
        }

        // Рекурсивные операции
        public abstract int GetSize();
        public abstract void Delete();
        public abstract FileSystemNode Copy();
        public abstract void Print(string indent = "");
    }
}