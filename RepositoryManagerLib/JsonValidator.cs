using System.Text.Json;

namespace RepositoryManagerLib
{
    public class JsonValidator : IContentValidator
    {
        public void Validate(string content)
        {
            try
            {
                JsonDocument.Parse(content); // throws if invalid JSON
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid JSON content", ex);
            }
        }
    }
}