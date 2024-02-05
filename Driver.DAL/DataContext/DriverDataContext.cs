using System.Data.SQLite;

namespace Driver.DAL.DataContext
{
    public class DriverDataContext
    {
        private static DriverDataContext _context;
        private static string _connectionString;
        public SQLiteConnection connection { get; set; }

        private DriverDataContext()
        {

            connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var command = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS driver (
                                                    ID TEXT PRIMARY KEY UNIQUE,
                                                    FirstName TEXT NOT NULL,
                                                    LastName TEXT NOT NULL,
                                                    Email TEXT NOT NULL,
                                                    PhoneNumber TEXT NOT NULL);", connection);
            command.ExecuteNonQuery();
        }

        public static DriverDataContext GetInstance(string? connectionString = null)
        {
            if (_context == null)
            {
                if (connectionString == null)
                {
                    throw new ArgumentNullException("connectionString cannot be null on initilize");
                }
                _connectionString = connectionString;
                _context = new DriverDataContext();
            }
            return _context;
        }

        ~DriverDataContext()
        {
            if (connection != null && connection.State > 0)
            {
                connection.Close();
            }
        }
    }
}
