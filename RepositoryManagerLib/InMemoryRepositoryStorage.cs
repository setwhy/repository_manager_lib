using System.Collections.Concurrent;

namespace RepositoryManagerLib
{
    public class InMemoryRepositoryStorage : IRepositoryStorage
    {
        private readonly ConcurrentDictionary<string, (string Content, ItemType Type)> _storage
            = new ConcurrentDictionary<string, (string, ItemType)>();

        public bool TryAdd(string key, string content, ItemType type)
        {
            return _storage.TryAdd(key, (content, type));
        }

        public bool TryGet(string key, out (string Content, ItemType Type) item)
        {
            return _storage.TryGetValue(key, out item);
        }

        public bool TryRemove(string key)
        {
            return _storage.TryRemove(key, out _);
        }
    }
}