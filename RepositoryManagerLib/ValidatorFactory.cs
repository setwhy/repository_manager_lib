namespace RepositoryManagerLib
{
    // Factory for validators (easy to extend later)
    public static class ValidatorFactory
    {
        public static IContentValidator GetValidator(ItemType type)
        {
            return type switch
            {
                ItemType.Json => new JsonValidator(),
                ItemType.Xml => new XmlValidator(),
                _ => throw new NotSupportedException($"Unsupported item type: {type}")
            };
        }
    }
}