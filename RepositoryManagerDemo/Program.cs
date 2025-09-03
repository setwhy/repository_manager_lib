using System;
using RepositoryManagerLib;  // <-- this matches your library namespace

namespace RepositoryManagerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Repository Manager Demo ===");

            // var manager = new RepositoryManager();
            var connectionString = "Host=localhost;Username=odoo17;Password=odoo17;Database=repo_db";
            var manager = new RepositoryManager(new PostgresRepositoryStorage(connectionString));
            manager.Initialize();

            string jsonItem = "{ \"id\": 1, \"name\": \"Wahyu\" }";
            string xmlItem = "<user><id>1</id><name>Setiawati</name></user>";

            try
            {
                // Register items
                Console.WriteLine("Registering JSON item1");
                manager.Register("item1", jsonItem, (int)ItemType.Json);
                Console.WriteLine("Registering XML item2");
                manager.Register("item2", xmlItem, (int)ItemType.Xml);

                // Try to overwrite (should fail)
                try
                {
                    Console.WriteLine("Rewriting item1 with new content");
                    manager.Register("item1", "{ \"id\": 2, \"name\": \"Bob\" }", (int)ItemType.Json);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Expected error: {ex.Message}");
                }

                // Retrieve items
                Console.WriteLine("Retrieve item1: " + manager.Retrieve("item1"));
                Console.WriteLine("Retrieve item2: " + manager.Retrieve("item2"));

                // Get item types
                Console.WriteLine("Item1 type: " + (ItemType)manager.GetType("item1"));
                Console.WriteLine("Item2 type: " + (ItemType)manager.GetType("item2"));

                // Deregister
                Console.WriteLine("Deregistering item1");
                manager.Deregister("item1");

                // Try to retrieve deleted item
                try
                {
                    Console.WriteLine("Retrieve item1 after delete: ");
                    manager.Retrieve("item1");
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine($"Expected error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex}");
            }

            Console.WriteLine("=== Demo Finished ===");
        }
    }
}