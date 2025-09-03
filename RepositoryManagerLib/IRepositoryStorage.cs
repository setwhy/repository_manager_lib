namespace RepositoryManagerLib
{
    // Abstraction for storage (e.g., can swap memory with DB/file later)
    public interface IRepositoryStorage
    {
        bool TryAdd(string key, string content, ItemType type);
        bool TryGet(string key, out (string Content, ItemType Type) item);
        bool TryRemove(string key);
    }
}