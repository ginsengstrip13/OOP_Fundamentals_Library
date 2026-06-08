using System.Collections.Generic;

namespace lab5.Patterns.Adapter
{
    public sealed class CloudDriveAdapter : IFileSystem
    {
        private readonly CloudDriveApi _api;

        public CloudDriveAdapter(CloudDriveApi api)
        {
            _api = api;
        }

        public IReadOnlyList<string> ListItems(string path)
        {
            return _api.SearchObjects(path);
        }

        public byte[] ReadFile(string path)
        {
            return _api.DownloadObject(path);
        }

        public void WriteFile(string path, byte[] data)
        {
            _api.UploadObject(path, data);
        }

        public void DeleteItem(string path)
        {
            _api.TrashObject(path);
        }
    }
}
