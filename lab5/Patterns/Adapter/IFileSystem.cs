using System.Collections.Generic;

namespace lab5.Patterns.Adapter
{
    public interface IFileSystem
    {
        IReadOnlyList<string> ListItems(string path);

        byte[] ReadFile(string path);

        void WriteFile(string path, byte[] data);

        void DeleteItem(string path);
    }
}
