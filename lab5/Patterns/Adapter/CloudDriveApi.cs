using System;
using System.Collections.Generic;

namespace lab5.Patterns.Adapter
{
    public sealed class CloudDriveApi
    {
        private readonly InMemoryPathStore _objects = new();

        public IReadOnlyList<string> SearchObjects(string folderKey)
        {
            Console.WriteLine($"[Cloud API] SearchObjects({folderKey})");
            return _objects.List(folderKey);
        }

        public byte[] DownloadObject(string objectKey)
        {
            Console.WriteLine($"[Cloud API] DownloadObject({objectKey})");
            return _objects.Read(objectKey);
        }

        public string UploadObject(string objectKey, byte[] content)
        {
            Console.WriteLine($"[Cloud API] UploadObject({objectKey}, {content.Length} байт)");
            _objects.Write(objectKey, content);
            return objectKey;
        }

        public void TrashObject(string objectKey)
        {
            Console.WriteLine($"[Cloud API] TrashObject({objectKey})");
            _objects.Delete(objectKey);
        }
    }
}
