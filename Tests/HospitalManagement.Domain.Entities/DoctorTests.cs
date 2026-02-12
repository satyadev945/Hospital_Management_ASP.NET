using System;
using System.Collections.Generic;
using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests
{
    public class DoctorTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var doctor = new Doctor();

            // Assert
            Assert.Equal(0, doctor.DoctorID);
            Assert.Equal(string.Empty, doctor.Name);
            Assert.Equal(string.Empty, doctor.Gender);
            Assert.Equal(string.Empty, doctor.Email);
            Assert.Equal(string.Empty, doctor.Password);
            Assert.Equal(string.Empty, doctor.Qualification);
            Assert.Equal(0, doctor.PatientsTreated);
            Assert.Equal(1, doctor.Status);
            Assert.True(doctor.IsActive);
            Assert.NotNull(doctor.Appointments);
            Assert.NotNull(doctor.TreatmentHistories);
            Assert.NotNull(doctor.Feedbacks);
        }

        [Fact]
        public void DoctorID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedId = 100;

            // Act
            doctor.DoctorID = expectedId;

            // Assert
            Assert.Equal(expectedId, doctor.DoctorID);
        }

        [Fact]
        public void Name_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedName = "Dr. Smith";

            // Act
            doctor.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, doctor.Name);
        }

        [Fact]
        public void Phone_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedPhone = "9876543210";

            // Act
            doctor.Phone = expectedPhone;

            // Assert
            Assert.Equal(expectedPhone, doctor.Phone);
        }

        [Fact]
        public void Address_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedAddress = "123 Medical St";

            // Act
            doctor.Address = expectedAddress;

            // Assert
            Assert.Equal(expectedAddress, doctor.Address);
        }

        [Fact]
        public void BirthDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedBirthDate = new DateTime(1980, 5, 15);

            // Act
            doctor.BirthDate = expectedBirthDate;

            // Assert
            Assert.Equal(expectedBirthDate, doctor.BirthDate);
        }

        [Fact]
        public void Gender_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedGender = "M";

            // Act
            doctor.Gender = expectedGender;

            // Assert
            Assert.Equal(expectedGender, doctor.Gender);
        }

        [Fact]
        public void DeptNo_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedDeptNo = 5;

            // Act
            doctor.DeptNo = expectedDeptNo;

            // Assert
            Assert.Equal(expectedDeptNo, doctor.DeptNo);
        }

        [Fact]
        public void ChargesPerVisit_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedCharges = 200.00m;

            // Act
            doctor.ChargesPerVisit = expectedCharges;

            // Assert
            Assert.Equal(expectedCharges, doctor.ChargesPerVisit);
        }

        [Fact]
        public void Charges_ShouldReturnChargesPerVisit()
        {
            // Arrange
            var doctor = new Doctor
            {
                ChargesPerVisit = 200.00m
            };

            // Act
            var charges = doctor.Charges;

            // Assert
            Assert.Equal(200.00m, charges);
        }

        [Fact]
        public void MonthlySalary_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedSalary = 100000.00m;

            // Act
            doctor.MonthlySalary = expectedSalary;

            // Assert
            Assert.Equal(expectedSalary, doctor.MonthlySalary);
        }

        [Fact]
        public void ReputeIndex_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedIndex = 4.5m;

            // Act
            doctor.ReputeIndex = expectedIndex;

            // Assert
            Assert.Equal(expectedIndex, doctor.ReputeIndex);
        }

        [Fact]
        public void PatientsTreated_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedCount = 150;

            // Act
            doctor.PatientsTreated = expectedCount;

            // Assert
            Assert.Equal(expectedCount, doctor.PatientsTreated);
        }

        [Fact]
        public void Qualification_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedQualification = "MBBS, MD";

            // Act
            doctor.Qualification = expectedQualification;

            // Assert
            Assert.Equal(expectedQualification, doctor.Qualification);
        }

        [Fact]
        public void Specialization_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedSpecialization = "Cardiology";

            // Act
            doctor.Specialization = expectedSpecialization;

            // Assert
            Assert.Equal(expectedSpecialization, doctor.Specialization);
        }

        [Fact]
        public void WorkExperience_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedExperience = 10;

            // Act
            doctor.WorkExperience = expectedExperience;

            // Assert
            Assert.Equal(expectedExperience, doctor.WorkExperience);
        }

        [Fact]
        public void Status_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedStatus = 0;

            // Act
            doctor.Status = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, doctor.Status);
        }

        [Fact]
        public void Email_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedEmail = "doctor@hospital.com";

            // Act
            doctor.Email = expectedEmail;

            // Assert
            Assert.Equal(expectedEmail, doctor.Email);
        }

        [Fact]
        public void Password_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedPassword = "hashedPassword123";

            // Act
            doctor.Password = expectedPassword;

            // Assert
            Assert.Equal(expectedPassword, doctor.Password);
        }

        [Fact]
        public void Age_ShouldCalculateCorrectly()
        {
            // Arrange
            var doctor = new Doctor
            {
                BirthDate = new DateTime(1980, 1, 1)
            };

            // Act
            var age = doctor.Age;

            // Assert
            Assert.True(age >= 44 && age <= 46);
        }

        [Fact]
        public void IsActive_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.IsActive = false;

            // Assert
            Assert.False(doctor.IsActive);
        }

        [Fact]
        public void CreatedDate_ShouldBeInitialized()
        {
            // Arrange & Act
            var doctor = new Doctor();

            // Assert
            Assert.True(doctor.CreatedDate <= DateTime.UtcNow);
            Assert.True(doctor.CreatedDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void ModifiedDate_ShouldBeNullByDefault()
        {
            // Arrange & Act
            var doctor = new Doctor();

            // Assert
            Assert.Null(doctor.ModifiedDate);
        }

        [Fact]
        public void Appointments_ShouldInitializeAsEmptyList()
        {
            // Arrange & Act
            var doctor = new Doctor();

            // Assert
            Assert.NotNull(doctor.Appointments);
            Assert.Empty(doctor.Appointments);
        }

        [Fact]
        public void TreatmentHistories_ShouldInitializeAsEmptyList()
        {
            // Arrange & Act
            var doctor = new Doctor();

            // Assert
            Assert.NotNull(doctor.TreatmentHistories);
            Assert.Empty(doctor.TreatmentHistories);
        }

        [Fact]
        public void Feedbacks_ShouldInitializeAsEmptyList()
        {
            // Arrange & Act
            var doctor = new Doctor();

            // Assert
            Assert.NotNull(doctor.Feedbacks);
            Assert.Empty(doctor.Feedbacks);
        }

        [Fact]
        public void CreatedBy_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedCreatedBy = "admin";

            // Act
            doctor.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, doctor.CreatedBy);
        }

        [Fact]
        public void ModifiedBy_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var doctor = new Doctor();
            var expectedModifiedBy = "admin";

            // Act
            doctor.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, doctor.ModifiedBy);
        }
    }
}
