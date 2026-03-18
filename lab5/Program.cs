using System;
using lab5.Patterns.Composite;
using lab5.Patterns.Facade;

namespace lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Создаем структуру (Компоновщик)
            var rootDir = new DirectoryItem("Root");
            var docsDir = new DirectoryItem("Documents");
            var picsDir = new DirectoryItem("Pictures");

            docsDir.Add(new FileItem("Report.docx", 1024));
            docsDir.Add(new FileItem("Data.csv", 2048));

            picsDir.Add(new FileItem("Avatar.png", 512));
            var vacationPics = new DirectoryItem("Vacation");
            vacationPics.Add(new FileItem("Beach.jpg", 4096));
            picsDir.Add(vacationPics);

            rootDir.Add(docsDir);
            rootDir.Add(picsDir);

            Console.WriteLine("--- Текущая файловая структура ---");
            rootDir.Print();

            // 2. Использование Фасада для бэкапа
            var facade = new FileManagerFacade();
            facade.BackupFolderToCloud(rootDir, "my-backups/2026");

            // 3. Тест рекурсивного копирования
            Console.WriteLine("\n--- Тест копирования папки Documents ---");
            var copiedDocs = docsDir.Copy();
            copiedDocs.Print();

            // 4. Тест рекурсивного удаления
            Console.WriteLine("\n--- Тест удаления папки Pictures ---");
            picsDir.Delete();

            Console.ReadLine(); // Чтобы консоль не закрылась сразу
        }
    }
}