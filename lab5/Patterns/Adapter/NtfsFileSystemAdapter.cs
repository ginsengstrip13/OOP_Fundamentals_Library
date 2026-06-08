using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using lab5.Patterns.Composite;
using FsFile = lab5.Patterns.Composite.File;

namespace lab5.Patterns.Adapter
{
    public sealed class NtfsFileSystemAdapter : IFileSystem
    {
        private readonly Folder _root;

        public NtfsFileSystemAdapter(Folder root)
        {
            _root = root;
        }

        public IReadOnlyList<string> ListItems(string path)
        {
            var item = FindItem(path);
            if (item is not Folder folder)
            {
                throw new InvalidOperationException($"'{path}' является файлом, а не папкой.");
            }

            var result = folder.Children.Select(child => child.GetAbsolutePath()).ToList();
            Console.WriteLine($"[NTFS Adapter] ListItems({Normalize(path)}) -> {result.Count} элемент(ов)");
            return result;
        }

        public byte[] ReadFile(string path)
        {
            var item = FindItem(path);
            if (item is not FsFile file)
            {
                throw new InvalidOperationException($"'{path}' является папкой, ее нельзя прочитать как файл.");
            }

            Console.WriteLine($"[NTFS Adapter] ReadFile({Normalize(path)})");
            return file.Read();
        }

        public void WriteFile(string path, byte[] data)
        {
            var normalized = Normalize(path);
            var segments = Split(normalized);
            var start = SkipRootSegment(segments);
            if (start >= segments.Length)
            {
                throw new InvalidOperationException("Путь записи должен содержать имя файла.");
            }

            var folder = _root;
            for (var i = start; i < segments.Length - 1; i++)
            {
                folder = GetOrCreateFolder(folder, segments[i]);
            }

            var fileName = segments[^1];
            var existing = folder.FindChild(fileName);
            if (existing is FsFile file)
            {
                file.Write(data);
            }
            else if (existing is null)
            {
                folder.Add(new FsFile(fileName, data));
            }
            else
            {
                throw new InvalidOperationException($"'{normalized}' указывает на папку, а не на файл.");
            }

            Console.WriteLine($"[NTFS Adapter] WriteFile({normalized}, {data.Length} байт)");
        }

        public void DeleteItem(string path)
        {
            var item = FindItem(path);
            Console.WriteLine($"[NTFS Adapter] DeleteItem({Normalize(path)})");
            item.Delete();
            item.Parent?.Remove(item);
        }

        private FileSystemItem FindItem(string path)
        {
            var segments = Split(Normalize(path));
            var start = SkipRootSegment(segments);
            FileSystemItem current = _root;

            for (var i = start; i < segments.Length; i++)
            {
                if (current is not Folder folder)
                {
                    throw new FileNotFoundException($"Путь '{path}' не найден.");
                }

                current = folder.FindChild(segments[i])
                    ?? throw new FileNotFoundException($"Путь '{path}' не найден.");
            }

            return current;
        }

        private Folder GetOrCreateFolder(Folder parent, string name)
        {
            var existing = parent.FindChild(name);
            if (existing is Folder folder)
            {
                return folder;
            }

            if (existing is not null)
            {
                throw new InvalidOperationException($"'{name}' уже существует как файл.");
            }

            var created = new Folder(name);
            parent.Add(created);
            return created;
        }

        private int SkipRootSegment(string[] segments)
        {
            return segments.Length > 0 && segments[0].Equals(_root.Name, StringComparison.OrdinalIgnoreCase)
                ? 1
                : 0;
        }

        private static string[] Split(string path)
        {
            return Normalize(path).Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        }

        private static string Normalize(string path)
        {
            return InMemoryPathStore.Normalize(path);
        }
    }
}
