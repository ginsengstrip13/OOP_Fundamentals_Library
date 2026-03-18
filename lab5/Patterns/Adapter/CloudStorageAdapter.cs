using lab5.Patterns.Composite;

namespace lab5.Patterns.Adapter
{
    
    public class CloudStorageAdapter : IStorage
    {
        private AmazonS3CloudAPI _cloudApi = new AmazonS3CloudAPI();

        public void ListContents(string path) => _cloudApi.FetchBucketItems(path);

        public void Write(string path, FileSystemNode node)
        {
            
            byte[] dummyData = new byte[node.GetSize()];
            _cloudApi.UploadObject($"https://s3.cloud.com/{path}", dummyData, node.Name);
        }

        public void Read(string path) => _cloudApi.DownloadObject($"https://s3.cloud.com/{path}");
        public void Delete(string path) => _cloudApi.RemoveObject($"https://s3.cloud.com/{path}");
    }
}