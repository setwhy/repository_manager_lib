using Npgsql;

namespace RepositoryManagerLib
{
    public class PostgresRepositoryStorage : IRepositoryStorage
    {
        private readonly string _connectionString;

        public PostgresRepositoryStorage(string connectionString)
        {
            _connectionString = connectionString;

            // Ensure table exists
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(@"
                CREATE TABLE IF NOT EXISTS repository_items (
                    item_name TEXT PRIMARY KEY,
                    content TEXT NOT NULL,
                    item_type INT NOT NULL
                );
            ", conn);
            cmd.ExecuteNonQuery();
        }

        public bool TryAdd(string key, string content, ItemType type)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            try
            {
                using var cmd = new NpgsqlCommand(
                    "INSERT INTO repository_items (item_name, content, item_type) VALUES (@name, @content, @type)", conn);
                cmd.Parameters.AddWithValue("name", key);
                cmd.Parameters.AddWithValue("content", content);
                cmd.Parameters.AddWithValue("type", (int)type);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // unique_violation
            {
                return false; // Item already exists
            }
        }

        public bool TryGet(string key, out (string Content, ItemType Type) item)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand(
                "SELECT content, item_type FROM repository_items WHERE item_name = @name", conn);
            cmd.Parameters.AddWithValue("name", key);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string content = reader.GetString(0);
                ItemType type = (ItemType)reader.GetInt32(1);
                item = (content, type);
                return true;
            }

            item = default;
            return false;
        }

        public bool TryRemove(string key)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM repository_items WHERE item_name = @name", conn);
            cmd.Parameters.AddWithValue("name", key);
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
