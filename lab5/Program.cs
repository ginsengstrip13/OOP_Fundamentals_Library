using System;
using lab5.Patterns.Adapter;
using lab5.Patterns.Composite;
using lab5.Patterns.Facade;
using FsFile = lab5.Patterns.Composite.File;

namespace lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var localRoot = BuildLocalFileTree();

            Console.WriteLine("=== Composite: исходная иерархия файловой системы ===");
            localRoot.Print();
            Console.WriteLine($"Размер корневой директории: {localRoot.GetSize()} байт");

            Console.WriteLine();
            Console.WriteLine("=== Composite: рекурсивное копирование папки Documents ===");
            var documents = (Folder)localRoot.GetChild(0)!;
            var documentsCopy = documents.Copy();
            documentsCopy.Name = "Documents_Copy";
            localRoot.Add(documentsCopy);
            localRoot.Print();

            var ntfs = new NtfsFileSystemAdapter(localRoot);
            var ftp = new FtpFileSystemAdapter(new LegacyFtpClient());
            var cloud = new CloudDriveAdapter(new CloudDriveApi());

            Console.WriteLine();
            Console.WriteLine("=== Adapter: единый интерфейс для NTFS, FTP и Cloud ===");
            PrintListing(ntfs, "/LocalDisk");

            var cloudSync = new SyncFacade(ntfs, cloud);
            cloudSync.SyncFolder("/LocalDisk/Documents", "/cloud-sync/Documents");
            cloudSync.Backup("/LocalDisk", "/backups/full-2026-06-08");
            PrintListing(cloud, "/backups/full-2026-06-08");

            var ftpBackup = new SyncFacade(ntfs, ftp);
            ftpBackup.Backup("/LocalDisk/Pictures", "/ftp-backup/Pictures");
            PrintListing(ftp, "/ftp-backup/Pictures");

            Console.WriteLine();
            Console.WriteLine("=== Composite: рекурсивное удаление папки Pictures ===");
            var pictures = (Folder)localRoot.GetChild(1)!;
            pictures.Delete();
            localRoot.Remove(pictures);
            localRoot.Print();
        }

        private static Folder BuildLocalFileTree()
        {
            var root = new Folder("LocalDisk");
            var documents = new Folder("Documents");
            var pictures = new Folder("Pictures");
            var projects = new Folder("Projects");
            var vacation = new Folder("Vacation");

            documents.Add(new FsFile("Report.docx", 1024));
            documents.Add(new FsFile("Data.csv", 2048));
            projects.Add(new FsFile("FileManager.cs", 4096));
            projects.Add(new FsFile("Patterns.md", 512));

            pictures.Add(new FsFile("Avatar.png", 512));
            vacation.Add(new FsFile("Beach.jpg", 4096));
            vacation.Add(new FsFile("Mountains.jpg", 3072));
            pictures.Add(vacation);

            root.Add(documents);
            root.Add(pictures);
            root.Add(projects);

            return root;
        }

        private static void PrintListing(IFileSystem fileSystem, string path)
        {
            Console.WriteLine($"Содержимое '{path}':");
            foreach (var item in fileSystem.ListItems(path))
            {
                Console.WriteLine($"  {item}");
            }
        }
    }
}
