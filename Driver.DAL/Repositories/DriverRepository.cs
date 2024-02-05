using Driver.DAL.DataContext;
using Driver.DAL.Enitities;
using Driver.DAL.Repositories.IRepositories;
using System.Linq;
using System.Data.SQLite;
using System.Diagnostics;
using System.Data;

namespace Driver.DAL.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private DriverDataContext _driverDataContext;

        public DriverRepository(DriverDataContext driverDataContext)
        {
            this._driverDataContext = driverDataContext;
        }

        public async Task<DriverEnitity> CreateDriverAsync(DriverEnitity driver)
        {
            driver.Id = Guid.NewGuid();
            var command = new SQLiteCommand("INSERT INTO Driver (Id, FirstName, LastName, Email, PhoneNumber) VALUES (@id, @firstName, @lastName, @email, @phoneNumber)", _driverDataContext.connection);
            command.Parameters.AddWithValue("@id", driver.Id.ToString());
            command.Parameters.AddWithValue("@firstName", driver.FirstName);
            command.Parameters.AddWithValue("@lastName", driver.LastName);
            command.Parameters.AddWithValue("@email", driver.Email);
            command.Parameters.AddWithValue("@phoneNumber", driver.PhoneNumber);
            await command.ExecuteNonQueryAsync();
            return driver;
        }

        public async Task DeleteDriverAsync(Guid id)
        {
            if ((await GetDriverByIdAsync(id)) is null)
            {
                throw new Exception("there is no driver with this ID");
            }
            else
            {
                var command = new SQLiteCommand($"DELETE FROM Driver WHERE ID = '{id.ToString()}';", _driverDataContext.connection);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<DriverEnitity>> GetAllDriversAsync()
        {
            var command = new SQLiteCommand("SELECT * FROM Driver", _driverDataContext.connection);
            var reader = await command.ExecuteReaderAsync();
            List<DriverEnitity> driverEnitities = reader.Cast<IDataRecord>().Select(x =>
            {
                var driver = new DriverEnitity((string)x["FirstName"], (string)x["LastName"], (string)x["Email"], (string)x["PhoneNumber"]);
                driver.Id = Guid.Parse((string)x["ID"]);
                return driver;
            }).ToList();

            return driverEnitities;
        }

        public async Task<List<DriverEnitity>> GetAlphabetizedDriversAsync()
        {
            var command = new SQLiteCommand("SELECT * FROM Driver ORDER BY FirstName, LastName;", _driverDataContext.connection);
            var reader = await command.ExecuteReaderAsync();
            List<DriverEnitity> driverEnitities = reader.Cast<IDataRecord>().Select(x =>
            {
                var driver = new DriverEnitity((string)x["FirstName"], (string)x["LastName"], (string)x["Email"], (string)x["PhoneNumber"]);
                driver.Id = Guid.Parse((string)x["ID"]);
                return driver;
            }).ToList();

            return driverEnitities;
        }

        public async Task<string> GetAlphabetizedNameAsync(Guid id)
        {
            var driver = await GetDriverByIdAsync(id);

            if (driver is null)
            {
                throw new Exception("there is no driver with this ID");
            }
            else
            {
                return $"{AlphabetizeWord(driver.FirstName)} {AlphabetizeWord(driver.LastName)}";
            }
        }

        public async Task<DriverEnitity?> GetDriverByIdAsync(Guid id)
        {
            var command = new SQLiteCommand($"SELECT * FROM Driver Where ID = '{id.ToString()}'", _driverDataContext.connection);
            var reader = await command.ExecuteReaderAsync();
            DriverEnitity? driverEnitity = reader.Cast<IDataRecord>().Select(x =>
            {
                var driver = new DriverEnitity((string)x["FirstName"], (string)x["LastName"], (string)x["Email"], (string)x["PhoneNumber"]);
                driver.Id = Guid.Parse((string)x["ID"]);
                return driver;
            }).FirstOrDefault();

            return driverEnitity;
        }

        public async Task<DriverEnitity> UpdateDriverAsync(DriverEnitity driver)
        {
            if ((await GetDriverByIdAsync(driver.Id)) is null)
            {
                throw new Exception("there is no driver with this ID");
            }
            else
            {
                var command = new SQLiteCommand("UPDATE driver SET FirstName = '@firstName', LastName = '@lastName', Email = '@email', PhoneNumber = '@phoneNumber' WHERE ID = '@id';", _driverDataContext.connection);
                command.Parameters.AddWithValue("@id", driver.Id.ToString());
                command.Parameters.AddWithValue("@firstName", driver.FirstName);
                command.Parameters.AddWithValue("@lastName", driver.LastName);
                command.Parameters.AddWithValue("@email", driver.Email);
                command.Parameters.AddWithValue("@phoneNumber", driver.PhoneNumber);
                await command.ExecuteNonQueryAsync();
                return driver;
            }
        }
    
        private string AlphabetizeWord(string word)
        {
            char[] characters = word.ToCharArray();
            Array.Sort(characters, (a, b) => char.ToLowerInvariant(a).CompareTo(char.ToLowerInvariant(b)));
            return new string(characters);
        }
    }
}
