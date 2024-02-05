
using Driver.DAL.Enitities;
using Xunit;

namespace Driver.Test.Unit_Test.Validation
{
    public class DriverTest
    {
        [Fact]
        public void Driver_Creates_WithValidProperties()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";

            // Act
            var driver = new DriverEnitity(firstName, lastName, email, phoneNumber);

            // Assert
            Assert.Equal(firstName, driver.FirstName);
            Assert.Equal(lastName, driver.LastName);
            Assert.Equal(email, driver.Email);
            Assert.Equal(phoneNumber, driver.PhoneNumber);
        }

        [Fact]
        public void Driver_Throws_On_EmptyFirstName()
        {
            // Arrange
            var lastName = "Doe";
            var email = "john.doe@example.com";
            var phoneNumber = "123-456-7890";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new DriverEnitity("", lastName, email, phoneNumber));
        }
    }
}
