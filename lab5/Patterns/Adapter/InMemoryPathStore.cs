using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace lab5.Patterns.Adapter
{
    internal sealed class InMemoryPathStore
    {
        private readonly Dictionary<string, byte[]> _files = new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<string> List(string path)
        {
            var normalized = Normalize(path);
            if (_files.ContainsKey(normalized))
            {
                throw new InvalidOperationException($"'{normalized}' является файлом, а не папкой.");
            }

            var prefix = normalized == "/" ? "/" : normalized + "/";
            var children = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var filePath in _files.Keys)
            {
                if (!filePath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var rest = filePath[prefix.Length..];
                if (string.IsNullOrWhiteSpace(rest))
                {
                    continue;
                }

                var childName = rest.Split('/')[0];
                children.Add(prefix + childName);
            }

            return children.ToList();
        }

        public byte[] Read(string path)
        {
            var normalized = Normalize(path);
            if (!_files.TryGetValue(normalized, out var data))
            {
                throw new FileNotFoundException($"Файл '{normalized}' не найден.");
            }

            return data.ToArray();
        }

        public void Write(string path, byte[] data)
        {
            var normalized = Normalize(path);
            if (normalized == "/")
            {
                throw new InvalidOperationException("Нельзя записать файл в корневой путь без имени.");
            }

            _files[normalized] = data.ToArray();
        }

        public void Delete(string path)
        {
            var normalized = Normalize(path);
            var removed = _files.Remove(normalized);
            var prefix = normalized == "/" ? "/" : normalized + "/";

            foreach (var filePath in _files.Keys.Where(filePath => filePath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList())
            {
                _files.Remove(filePath);
                removed = true;
            }

            if (!removed)
            {
                throw new FileNotFoundException($"Элемент '{normalized}' не найден.");
            }
        }

        public static string Normalize(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "/";
            }

            var normalized = path.Replace('\\', '/').Trim();
            if (!normalized.StartsWith('/'))
            {
                normalized = "/" + normalized;
            }

            while (normalized.Contains("//", StringComparison.Ordinal))
            {
                normalized = normalized.Replace("//", "/", StringComparison.Ordinal);
            }

            return normalized.Length > 1 ? normalized.TrimEnd('/') : normalized;
        }
    }
}
