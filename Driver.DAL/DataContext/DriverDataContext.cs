using Driver.DAL.Enitities;
using System.Data;
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
             command = new SQLiteCommand("SELECT * FROM Driver", connection);
            var reader =  command.ExecuteReader();

            if (!reader.Cast<IDataRecord>().Any())
            {
                for (int i = 0; i < 11; i++)
                {
                    command = new SQLiteCommand("INSERT INTO Driver (Id, FirstName, LastName, Email, PhoneNumber) VALUES (@id, @firstName, @lastName, @email, @phoneNumber)", connection);
                    command.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
                    command.Parameters.AddWithValue("@firstName", GenerateRandomString(5));
                    command.Parameters.AddWithValue("@lastName", GenerateRandomString(7));
                    command.Parameters.AddWithValue("@email", GenerateRandomString(11));
                    command.Parameters.AddWithValue("@phoneNumber", GenerateRandomString(10));
                    command.ExecuteNonQuery();
                }
            }
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
        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var stringChars = new char[length];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
