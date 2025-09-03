namespace RepositoryManagerLib
{
    // Public Repository Manager
    public class RepositoryManager
    {
        private readonly IRepositoryStorage _storage;
        private bool _initialized = false;

        public RepositoryManager(IRepositoryStorage? storage = null)
        {
            _storage = storage ?? new InMemoryRepositoryStorage();
        }

        public void Initialize()
        {
            if (_initialized)
                throw new InvalidOperationException("Repository already initialized.");

            _initialized = true;
        }

        public void Register(string itemName, string itemContent, int itemType)
        {
            EnsureInitialized();

            var type = (ItemType)itemType;

            // Validate content
            var validator = ValidatorFactory.GetValidator(type);
            validator.Validate(itemContent);

            // Try to add (no overwrite allowed)
            if (!_storage.TryAdd(itemName, itemContent, type))
                throw new InvalidOperationException($"Item '{itemName}' already exists.");
        }

        public string Retrieve(string itemName)
        {
            EnsureInitialized();

            if (_storage.TryGet(itemName, out var item))
                return item.Content;

            throw new KeyNotFoundException($"Item '{itemName}' not found.");
        }

        public int GetType(string itemName)
        {
            EnsureInitialized();

            if (_storage.TryGet(itemName, out var item))
                return (int)item.Type;

            throw new KeyNotFoundException($"Item '{itemName}' not found.");
        }

        public void Deregister(string itemName)
        {
            EnsureInitialized();

            if (!_storage.TryRemove(itemName))
                throw new KeyNotFoundException($"Item '{itemName}' not found.");
        }

        private void EnsureInitialized()
        {
            if (!_initialized)
                throw new InvalidOperationException("Repository not initialized. Call Initialize() first.");
        }
    }
}