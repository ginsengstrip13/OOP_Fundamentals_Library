using System;
using System.Collections.Generic;
using System.IO;
using lab5.Patterns.Adapter;

namespace lab5.Patterns.Facade
{
    public sealed class SyncFacade
    {
        private readonly IFileSystem _sourceFileSystem;
        private readonly IFileSystem _targetFileSystem;

        public SyncFacade(IFileSystem sourceFileSystem, IFileSystem targetFileSystem)
        {
            _sourceFileSystem = sourceFileSystem;
            _targetFileSystem = targetFileSystem;
        }

        public void SyncFolder(string sourcePath, string targetPath)
        {
            Console.WriteLine();
            Console.WriteLine($"=== Синхронизация '{sourcePath}' -> '{targetPath}' ===");
            CopyRecursively(sourcePath, targetPath);
            Console.WriteLine("Синхронизация завершена.");
        }

        public void Backup(string sourcePath, string backupPath)
        {
            Console.WriteLine();
            Console.WriteLine($"=== Резервное копирование '{sourcePath}' -> '{backupPath}' ===");
            try
            {
                _targetFileSystem.DeleteItem(backupPath);
                Console.WriteLine($"Старая резервная копия '{backupPath}' удалена.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Предыдущая резервная копия не найдена, создается новая.");
            }

            CopyRecursively(sourcePath, backupPath);
            Console.WriteLine("Резервное копирование завершено.");
        }

        private void CopyRecursively(string sourcePath, string targetPath)
        {
            if (TryListItems(sourcePath, out var children))
            {
                if (children.Count == 0)
                {
                    Console.WriteLine($"Папка '{sourcePath}' пуста, файлов для копирования нет.");
                    return;
                }

                foreach (var childPath in children)
                {
                    var targetChildPath = Combine(targetPath, GetName(childPath));
                    CopyRecursively(childPath, targetChildPath);
                }

                return;
            }

            var data = _sourceFileSystem.ReadFile(sourcePath);
            _targetFileSystem.WriteFile(targetPath, data);
            Console.WriteLine($"Файл скопирован: {sourcePath} -> {targetPath} ({data.Length} байт)");
        }

        private bool TryListItems(string path, out IReadOnlyList<string> children)
        {
            try
            {
                children = _sourceFileSystem.ListItems(path);
                return true;
            }
            catch (InvalidOperationException)
            {
                children = [];
                return false;
            }
        }

        private static string Combine(string directory, string name)
        {
            var normalizedDirectory = Normalize(directory);
            return normalizedDirectory == "/" ? "/" + name : normalizedDirectory + "/" + name;
        }

        private static string GetName(string path)
        {
            var normalized = Normalize(path);
            var slashIndex = normalized.LastIndexOf('/');
            return slashIndex >= 0 ? normalized[(slashIndex + 1)..] : normalized;
        }

        private static string Normalize(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "/";
            }

            var normalized = path.Replace('\\', '/').Trim();
            if (!normalized.StartsWith('/'))
            {
                normalized = "/" + normalized;
            }

            return normalized.Length > 1 ? normalized.TrimEnd('/') : normalized;
        }
    }
}
