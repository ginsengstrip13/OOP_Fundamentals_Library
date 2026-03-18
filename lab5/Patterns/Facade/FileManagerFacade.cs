using System;
using lab5.Patterns.Composite;
using lab5.Patterns.Adapter;

namespace lab5.Patterns.Facade
{
    // Фасад, предоставляющий простой интерфейс для сложных операций
    public class FileManagerFacade
    {
        private IStorage _local = new LocalStorage();
        private IStorage _cloud = new CloudStorageAdapter();

        public void BackupFolderToCloud(DirectoryItem folder, string cloudPath)
        {
            Console.WriteLine("\n=== СТАРТ РЕЗЕРВНОГО КОПИРОВАНИЯ В ОБЛАКО ===");
            Console.WriteLine($"Анализ папки '{folder.Name}'...");
            int totalSize = folder.GetSize();
            Console.WriteLine($"Общий размер для бэкапа: {totalSize} байт");

            Console.WriteLine("\nНачинаем синхронизацию (через Адаптер):");
            _cloud.Write(cloudPath, folder);
            Console.WriteLine("=== БЭКАП УСПЕШНО ЗАВЕРШЕН ===\n");
        }

        public void SyncCloudToLocal(string cloudPath, string localPath)
        {
            Console.WriteLine("\n=== СТАРТ СИНХРОНИЗАЦИИ ОБЛАКО -> ЛОКАЛЬНЫЙ ДИСК ===");
            _cloud.ListContents(cloudPath);
            _cloud.Read(cloudPath);
            _local.Write(localPath, new FileItem("SyncData.zip", 5000));
            Console.WriteLine("=== СИНХРОНИЗАЦИЯ ЗАВЕРШЕНА ===\n");
        }
    }
}