using System;
using lab5.Patterns.Composite;

namespace lab5.Patterns.Adapter
{
    
    public class LocalStorage : IStorage
    {
        public void ListContents(string path) => Console.WriteLine($"[Локальный Диск] Просмотр: {path}");
        public void Write(string path, FileSystemNode node) => Console.WriteLine($"[Локальный Диск] Запись узла '{node.Name}' по пути {path}");
        public void Read(string path) => Console.WriteLine($"[Локальный Диск] Чтение из {path}");
        public void Delete(string path) => Console.WriteLine($"[Локальный Диск] Удаление {path}");
    }
}