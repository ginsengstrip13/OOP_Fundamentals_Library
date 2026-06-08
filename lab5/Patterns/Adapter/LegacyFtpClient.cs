using System;
using System.Collections.Generic;

namespace lab5.Patterns.Adapter
{
    public sealed class LegacyFtpClient
    {
        private readonly InMemoryPathStore _store = new();

        public IReadOnlyList<string> GetDirectoryEntries(string directory)
        {
            Console.WriteLine($"[FTP API] NLST {directory}");
            return _store.List(directory);
        }

        public byte[] DownloadBinary(string fileName)
        {
            Console.WriteLine($"[FTP API] RETR {fileName}");
            return _store.Read(fileName);
        }

        public void UploadBinary(string fileName, byte[] payload)
        {
            Console.WriteLine($"[FTP API] STOR {fileName}");
            _store.Write(fileName, payload);
        }

        public void RemoveResource(string resourceName)
        {
            Console.WriteLine($"[FTP API] DELETE {resourceName}");
            _store.Delete(resourceName);
        }
    }
}
