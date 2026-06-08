using System.Collections.Generic;

namespace lab5.Patterns.Adapter
{
    public sealed class FtpFileSystemAdapter : IFileSystem
    {
        private readonly LegacyFtpClient _client;

        public FtpFileSystemAdapter(LegacyFtpClient client)
        {
            _client = client;
        }

        public IReadOnlyList<string> ListItems(string path)
        {
            return _client.GetDirectoryEntries(path);
        }

        public byte[] ReadFile(string path)
        {
            return _client.DownloadBinary(path);
        }

        public void WriteFile(string path, byte[] data)
        {
            _client.UploadBinary(path, data);
        }

        public void DeleteItem(string path)
        {
            _client.RemoveResource(path);
        }
    }
}
