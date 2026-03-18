using lab5.Patterns.Composite;

namespace lab5.Patterns.Adapter
{
    
    public interface IStorage
    {
        void ListContents(string path);
        void Write(string path, FileSystemNode node);
        void Read(string path);
        void Delete(string path);
    }
}