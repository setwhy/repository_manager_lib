using System.Xml;

namespace RepositoryManagerLib
{
    public class XmlValidator : IContentValidator
    {
        public void Validate(string content)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(content); // throws if invalid XML
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid XML content", ex);
            }
        }
    }
}