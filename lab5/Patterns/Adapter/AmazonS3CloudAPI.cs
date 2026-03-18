using System;

namespace lab5.Patterns.Adapter
{
    
    public class AmazonS3CloudAPI
    {
        public void FetchBucketItems(string bucketName) => Console.WriteLine($"[Cloud API] Получение объектов из бакета {bucketName}");
        public void UploadObject(string endpoint, byte[] data, string objName) => Console.WriteLine($"[Cloud API] Загрузка объекта '{objName}' на сервер {endpoint}");
        public void DownloadObject(string endpoint) => Console.WriteLine($"[Cloud API] Скачивание объекта с {endpoint}");
        public void RemoveObject(string endpoint) => Console.WriteLine($"[Cloud API] Уничтожение объекта по ссылке {endpoint}");
    }
}