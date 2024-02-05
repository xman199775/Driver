using Driver.DAL.DataContext;
using Driver.DAL.Enitities;
using Driver.DAL.Repositories;
using Driver.DAL.Repositories.IRepositories;
using System.Data.SQLite;
using Xunit;

namespace Driver.Test.Unit_Test.Repositories
{
    public class DriverRepositoryTest
    {
        private readonly IDriverRepository _driverRepository;
        private DriverDataContext _driverDataContext;
        public DriverRepositoryTest() {
            _driverDataContext = DriverDataContext.GetInstance("DataSource=:memory:");
           _driverRepository = new DriverRepository(_driverDataContext);
        }

        [Fact]
        public async Task Create_Driver_Async()
        {  
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);
            
            // Act
            var driverResult = await this._driverRepository.CreateDriverAsync(driver);

            Assert.NotEqual<Guid>(driverResult.Id, Guid.Empty);
            Assert.Equal(firstName, driverResult.FirstName);
            Assert.Equal(lastName, driverResult.LastName);
            Assert.Equal(email, driverResult.Email);
            Assert.Equal(phoneNumber, driverResult.PhoneNumber);
        }

        [Fact]
        public async Task Get_All_Drivers_Async()
        {
            // Arrange
            var command = new SQLiteCommand($"DELETE FROM Driver ;", _driverDataContext.connection);
            await command.ExecuteNonQueryAsync();
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);
            await this._driverRepository.CreateDriverAsync(driver);
            driver.Email = "john2.doe@example.com";
            await this._driverRepository.CreateDriverAsync(driver);
            // Act
            var result = await this._driverRepository.GetAllDriversAsync();

            Assert.IsType<List<DriverEnitity>>( result);
            Assert.Equal(2, result.Count());
        }
        
        [Fact]
        public async Task Get_Driver_By_Id_Async_Not_Found()
        {
            // Arrange
            var command = new SQLiteCommand($"DELETE FROM Driver ;", _driverDataContext.connection);
            await command.ExecuteNonQueryAsync();
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);
            await this._driverRepository.CreateDriverAsync(driver);
            driver.Email = "john2.doe@example.com";
            var driver2 = await this._driverRepository.CreateDriverAsync(driver);
            // Act
            var result = await this._driverRepository.GetDriverByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task Get_Driver_By_Id_Async()
        {
            // Arrange
            var command = new SQLiteCommand($"DELETE FROM Driver ;", _driverDataContext.connection);
            await command.ExecuteNonQueryAsync();
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);
            await this._driverRepository.CreateDriverAsync(driver);
            driver.Email = "john2.doe@example.com";
            var driver2 = await this._driverRepository.CreateDriverAsync(driver);
            // Act
            var result = await this._driverRepository.GetDriverByIdAsync(driver.Id);

            Assert.IsType<DriverEnitity>(result);
            Assert.Equal(driver2.FirstName, result.FirstName);
            Assert.Equal(driver2.LastName, result.LastName);
            Assert.Equal(driver2.Email, result.Email);
            Assert.Equal(driver2.PhoneNumber, result.PhoneNumber);
        }

        [Fact]
        public async Task Delete_Driver_Async_Not_found()
        {
            // Arrange
            var command = new SQLiteCommand($"DELETE FROM Driver ;", _driverDataContext.connection);
            await command.ExecuteNonQueryAsync();
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);
            await this._driverRepository.CreateDriverAsync(driver);
            driver.Email = "john2.doe@example.com";
            var driver2 = await this._driverRepository.CreateDriverAsync(driver);
            // Act
            await Assert.ThrowsAnyAsync<Exception>(async () => await _driverRepository.DeleteDriverAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task Delete_Driver_Async()
        {
            // Arrange
            var command = new SQLiteCommand($"DELETE FROM Driver ;", _driverDataContext.connection);
            await command.ExecuteNonQueryAsync();
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);
            await this._driverRepository.CreateDriverAsync(driver);
            driver.Email = "john2.doe@example.com";
            var driver2 = await this._driverRepository.CreateDriverAsync(driver);
            // Act
            await this._driverRepository.DeleteDriverAsync(driver2.Id);
            var result = await this._driverRepository.GetDriverByIdAsync(driver2.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task Update_Driver_Async_Not_found()
        {
            // Arrange
            var command = new SQLiteCommand($"DELETE FROM Driver ;", _driverDataContext.connection);
            await command.ExecuteNonQueryAsync();
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);
            var driverEntity = await this._driverRepository.CreateDriverAsync(driver);
            driverEntity.Email = "john2.doe@example.com";
            driverEntity.Id = Guid.NewGuid();
            // Act
            await Assert.ThrowsAnyAsync<Exception>(async () => await _driverRepository.UpdateDriverAsync(driverEntity));
        }

        [Fact]
        public async Task Update_Driver_Async()
        {
            // Arrange
            var command = new SQLiteCommand($"DELETE FROM Driver ;", _driverDataContext.connection);
            await command.ExecuteNonQueryAsync();
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);
            var driverEntity = await this._driverRepository.CreateDriverAsync(driver);
            driverEntity.Email = "john2.doe@example.com";
            // Act
            var result = await _driverRepository.UpdateDriverAsync(driverEntity);

            Assert.Equal("john2.doe@example.com",result.Email);
        }

        [Fact]
        public async Task Get_Alphabetized_Drivers_Async()
        {
            // Arrange
            var command = new SQLiteCommand($"DELETE FROM Driver ;", _driverDataContext.connection);
            await command.ExecuteNonQueryAsync();
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);
            await this._driverRepository.CreateDriverAsync(driver);
            driver.FirstName = "Ahmed";
            driver.LastName = "Sayed";
            await this._driverRepository.CreateDriverAsync(driver);
            // Act
            var result = await this._driverRepository.GetAlphabetizedDriversAsync();

            Assert.IsType<List<DriverEnitity>>(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Ahmed", result[0].FirstName);
            Assert.Equal("John", result[1].FirstName);
        }

        [Fact]
        public async Task Get_Alphabetized_Name_Async()
        {
            // Arrange
            var command = new SQLiteCommand($"DELETE FROM Driver ;", _driverDataContext.connection);
            await command.ExecuteNonQueryAsync();
            var firstName = "Oliver";
            var lastName = "Johnson";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);
            var driverResult = await this._driverRepository.CreateDriverAsync(driver);
            // Act
            var str = await this._driverRepository.GetAlphabetizedNameAsync(driverResult.Id);

            Assert.Equal("eilOrv hJnnoos", str);
        }

    }
}