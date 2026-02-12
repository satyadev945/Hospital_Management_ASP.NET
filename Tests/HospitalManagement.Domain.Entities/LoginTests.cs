using System;
using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests
{
    public class LoginTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var login = new Login();

            // Assert
            Assert.Equal(0, login.LoginID);
            Assert.Equal(string.Empty, login.Email);
            Assert.Equal(string.Empty, login.Password);
            Assert.Equal(0, login.Type);
            Assert.True(login.IsActive);
            Assert.Equal(0, login.FailedLoginAttempts);
        }

        [Fact]
        public void LoginID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var login = new Login();
            var expectedId = 100;

            // Act
            login.LoginID = expectedId;

            // Assert
            Assert.Equal(expectedId, login.LoginID);
        }

        [Fact]
        public void Email_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var login = new Login();
            var expectedEmail = "user@example.com";

            // Act
            login.Email = expectedEmail;

            // Assert
            Assert.Equal(expectedEmail, login.Email);
        }

        [Fact]
        public void Password_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var login = new Login();
            var expectedPassword = "hashedPassword123";

            // Act
            login.Password = expectedPassword;

            // Assert
            Assert.Equal(expectedPassword, login.Password);
        }

        [Fact]
        public void Type_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var login = new Login();
            var expectedType = 1;

            // Act
            login.Type = expectedType;

            // Assert
            Assert.Equal(expectedType, login.Type);
        }

        [Fact]
        public void IsPatient_ShouldReturnTrueWhenTypeIs1()
        {
            // Arrange
            var login = new Login
            {
                Type = 1
            };

            // Act
            var isPatient = login.IsPatient;

            // Assert
            Assert.True(isPatient);
        }

        [Fact]
        public void IsDoctor_ShouldReturnTrueWhenTypeIs2()
        {
            // Arrange
            var login = new Login
            {
                Type = 2
            };

            // Act
            var isDoctor = login.IsDoctor;

            // Assert
            Assert.True(isDoctor);
        }

        [Fact]
        public void IsAdmin_ShouldReturnTrueWhenTypeIs3()
        {
            // Arrange
            var login = new Login
            {
                Type = 3
            };

            // Act
            var isAdmin = login.IsAdmin;

            // Assert
            Assert.True(isAdmin);
        }

        [Fact]
        public void IsStaff_ShouldReturnTrueWhenTypeIs4()
        {
            // Arrange
            var login = new Login
            {
                Type = 4
            };

            // Act
            var isStaff = login.IsStaff;

            // Assert
            Assert.True(isStaff);
        }

        [Fact]
        public void IsActive_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var login = new Login();

            // Act
            login.IsActive = false;

            // Assert
            Assert.False(login.IsActive);
        }

        [Fact]
        public void FailedLoginAttempts_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var login = new Login();
            var expectedAttempts = 3;

            // Act
            login.FailedLoginAttempts = expectedAttempts;

            // Assert
            Assert.Equal(expectedAttempts, login.FailedLoginAttempts);
        }

        [Fact]
        public void LockoutEnd_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var login = new Login();
            var expectedLockoutEnd = DateTime.UtcNow.AddMinutes(30);

            // Act
            login.LockoutEnd = expectedLockoutEnd;

            // Assert
            Assert.Equal(expectedLockoutEnd, login.LockoutEnd);
        }

        [Fact]
        public void IsLockedOut_ShouldReturnTrueWhenLockoutEndIsInFuture()
        {
            // Arrange
            var login = new Login
            {
                LockoutEnd = DateTime.UtcNow.AddMinutes(10)
            };

            // Act
            var isLockedOut = login.IsLockedOut;

            // Assert
            Assert.True(isLockedOut);
        }

        [Fact]
        public void IsLockedOut_ShouldReturnFalseWhenLockoutEndIsInPast()
        {
            // Arrange
            var login = new Login
            {
                LockoutEnd = DateTime.UtcNow.AddMinutes(-10)
            };

            // Act
            var isLockedOut = login.IsLockedOut;

            // Assert
            Assert.False(isLockedOut);
        }

        [Fact]
        public void IsLockedOut_ShouldReturnFalseWhenLockoutEndIsNull()
        {
            // Arrange
            var login = new Login
            {
                LockoutEnd = null
            };

            // Act
            var isLockedOut = login.IsLockedOut;

            // Assert
            Assert.False(isLockedOut);
        }

        [Fact]
        public void LastLoginDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var login = new Login();
            var expectedDate = DateTime.UtcNow;

            // Act
            login.LastLoginDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, login.LastLoginDate);
        }

        [Fact]
        public void CreatedDate_ShouldBeInitialized()
        {
            // Arrange & Act
            var login = new Login();

            // Assert
            Assert.True(login.CreatedDate <= DateTime.UtcNow);
            Assert.True(login.CreatedDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void ModifiedDate_ShouldBeNullByDefault()
        {
            // Arrange & Act
            var login = new Login();

            // Assert
            Assert.Null(login.ModifiedDate);
        }

        [Fact]
        public void CreatedBy_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var login = new Login();
            var expectedCreatedBy = "system";

            // Act
            login.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, login.CreatedBy);
        }

        [Fact]
        public void ModifiedBy_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var login = new Login();
            var expectedModifiedBy = "admin";

            // Act
            login.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, login.ModifiedBy);
        }

        [Fact]
        public void IsPatient_ShouldReturnFalseForOtherTypes()
        {
            // Arrange
            var login = new Login
            {
                Type = 2
            };

            // Act
            var isPatient = login.IsPatient;

            // Assert
            Assert.False(isPatient);
        }

        [Fact]
        public void IsDoctor_ShouldReturnFalseForOtherTypes()
        {
            // Arrange
            var login = new Login
            {
                Type = 1
            };

            // Act
            var isDoctor = login.IsDoctor;

            // Assert
            Assert.False(isDoctor);
        }
    }
}
