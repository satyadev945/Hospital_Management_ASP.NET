using System;
using System.Collections.Generic;
using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests
{
    public class PatientTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.Equal(0, patient.PatientID);
            Assert.Equal(string.Empty, patient.Name);
            Assert.Equal(string.Empty, patient.Gender);
            Assert.Equal(string.Empty, patient.Email);
            Assert.Equal(string.Empty, patient.Password);
            Assert.True(patient.IsActive);
            Assert.NotNull(patient.Appointments);
            Assert.NotNull(patient.Bills);
            Assert.NotNull(patient.TreatmentHistories);
            Assert.NotNull(patient.Feedbacks);
        }

        [Fact]
        public void PatientID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedId = 123;

            // Act
            patient.PatientID = expectedId;

            // Assert
            Assert.Equal(expectedId, patient.PatientID);
        }

        [Fact]
        public void Name_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedName = "John Doe";

            // Act
            patient.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, patient.Name);
        }

        [Fact]
        public void Phone_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedPhone = "1234567890";

            // Act
            patient.Phone = expectedPhone;

            // Assert
            Assert.Equal(expectedPhone, patient.Phone);
        }

        [Fact]
        public void Address_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedAddress = "123 Main St";

            // Act
            patient.Address = expectedAddress;

            // Assert
            Assert.Equal(expectedAddress, patient.Address);
        }

        [Fact]
        public void BirthDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedBirthDate = new DateTime(1990, 1, 1);

            // Act
            patient.BirthDate = expectedBirthDate;

            // Assert
            Assert.Equal(expectedBirthDate, patient.BirthDate);
        }

        [Fact]
        public void Gender_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedGender = "M";

            // Act
            patient.Gender = expectedGender;

            // Assert
            Assert.Equal(expectedGender, patient.Gender);
        }

        [Fact]
        public void Email_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedEmail = "john.doe@example.com";

            // Act
            patient.Email = expectedEmail;

            // Assert
            Assert.Equal(expectedEmail, patient.Email);
        }

        [Fact]
        public void Password_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedPassword = "hashedPassword123";

            // Act
            patient.Password = expectedPassword;

            // Assert
            Assert.Equal(expectedPassword, patient.Password);
        }

        [Fact]
        public void Age_ShouldCalculateCorrectly()
        {
            // Arrange
            var patient = new Patient
            {
                BirthDate = new DateTime(1990, 1, 1)
            };

            // Act
            var age = patient.Age;

            // Assert
            Assert.True(age >= 34 && age <= 36);
        }

        [Fact]
        public void IsActive_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.IsActive = false;

            // Assert
            Assert.False(patient.IsActive);
        }

        [Fact]
        public void CreatedDate_ShouldBeInitialized()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.True(patient.CreatedDate <= DateTime.UtcNow);
            Assert.True(patient.CreatedDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void ModifiedDate_ShouldBeNullByDefault()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.Null(patient.ModifiedDate);
        }

        [Fact]
        public void ModifiedDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedDate = DateTime.UtcNow;

            // Act
            patient.ModifiedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, patient.ModifiedDate);
        }

        [Fact]
        public void CreatedBy_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedCreatedBy = "admin";

            // Act
            patient.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, patient.CreatedBy);
        }

        [Fact]
        public void ModifiedBy_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var patient = new Patient();
            var expectedModifiedBy = "admin";

            // Act
            patient.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, patient.ModifiedBy);
        }

        [Fact]
        public void Appointments_ShouldInitializeAsEmptyList()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.NotNull(patient.Appointments);
            Assert.Empty(patient.Appointments);
        }

        [Fact]
        public void Bills_ShouldInitializeAsEmptyList()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.NotNull(patient.Bills);
            Assert.Empty(patient.Bills);
        }

        [Fact]
        public void TreatmentHistories_ShouldInitializeAsEmptyList()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.NotNull(patient.TreatmentHistories);
            Assert.Empty(patient.TreatmentHistories);
        }

        [Fact]
        public void Feedbacks_ShouldInitializeAsEmptyList()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.NotNull(patient.Feedbacks);
            Assert.Empty(patient.Feedbacks);
        }

        [Fact]
        public void Age_ShouldHandleLeapYearBirthDate()
        {
            // Arrange
            var patient = new Patient
            {
                BirthDate = new DateTime(2000, 2, 29)
            };

            // Act
            var age = patient.Age;

            // Assert
            Assert.True(age >= 24 && age <= 26);
        }

        [Fact]
        public void Phone_CanBeNull()
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.Phone = null;

            // Assert
            Assert.Null(patient.Phone);
        }

        [Fact]
        public void Address_CanBeNull()
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.Address = null;

            // Assert
            Assert.Null(patient.Address);
        }
    }
}
