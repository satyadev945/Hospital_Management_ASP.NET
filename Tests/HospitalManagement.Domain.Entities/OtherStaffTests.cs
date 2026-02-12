using System;
using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests
{
    public class OtherStaffTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var staff = new OtherStaff();

            // Assert
            Assert.Equal(0, staff.StaffID);
            Assert.Equal(string.Empty, staff.Name);
            Assert.Equal(string.Empty, staff.Designation);
            Assert.Equal(string.Empty, staff.Gender);
            Assert.Equal("Active", staff.Status);
            Assert.True(staff.IsActive);
        }

        [Fact]
        public void StaffID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedId = 456;

            // Act
            staff.StaffID = expectedId;

            // Assert
            Assert.Equal(expectedId, staff.StaffID);
        }

        [Fact]
        public void Name_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedName = "Jane Smith";

            // Act
            staff.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, staff.Name);
        }

        [Fact]
        public void Designation_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedDesignation = "Nurse";

            // Act
            staff.Designation = expectedDesignation;

            // Assert
            Assert.Equal(expectedDesignation, staff.Designation);
        }

        [Fact]
        public void Age_ShouldCalculateCorrectlyWhenBirthDateIsSet()
        {
            // Arrange
            var staff = new OtherStaff
            {
                BirthDate = new DateTime(1985, 5, 15)
            };

            // Act
            var age = staff.Age;

            // Assert
            Assert.NotNull(age);
            Assert.True(age >= 39 && age <= 40);
        }

        [Fact]
        public void Age_ShouldBeNullWhenBirthDateIsNull()
        {
            // Arrange
            var staff = new OtherStaff
            {
                BirthDate = null
            };

            // Act
            var age = staff.Age;

            // Assert
            Assert.Null(age);
        }

        [Fact]
        public void IsCurrentlyEmployed_ShouldReturnTrueWhenStatusIsActive()
        {
            // Arrange
            var staff = new OtherStaff
            {
                Status = "Active"
            };

            // Act
            var isEmployed = staff.IsCurrentlyEmployed;

            // Assert
            Assert.True(isEmployed);
        }

        [Fact]
        public void IsCurrentlyEmployed_ShouldReturnFalseWhenStatusIsNotActive()
        {
            // Arrange
            var staff = new OtherStaff
            {
                Status = "Terminated"
            };

            // Act
            var isEmployed = staff.IsCurrentlyEmployed;

            // Assert
            Assert.False(isEmployed);
        }

        [Fact]
        public void YearsOfService_ShouldCalculateCorrectlyWhenJoiningDateIsSet()
        {
            // Arrange
            var staff = new OtherStaff
            {
                JoiningDate = DateTime.UtcNow.AddYears(-5)
            };

            // Act
            var yearsOfService = staff.YearsOfService;

            // Assert
            Assert.NotNull(yearsOfService);
            Assert.True(yearsOfService >= 4 && yearsOfService <= 5);
        }

        [Fact]
        public void YearsOfService_ShouldBeNullWhenJoiningDateIsNull()
        {
            // Arrange
            var staff = new OtherStaff
            {
                JoiningDate = null
            };

            // Act
            var yearsOfService = staff.YearsOfService;

            // Assert
            Assert.Null(yearsOfService);
        }

        [Fact]
        public void IsOnProbation_ShouldReturnTrueWhenLessThan180Days()
        {
            // Arrange
            var staff = new OtherStaff
            {
                JoiningDate = DateTime.UtcNow.AddDays(-100)
            };

            // Act
            var isOnProbation = staff.IsOnProbation;

            // Assert
            Assert.True(isOnProbation);
        }

        [Fact]
        public void IsOnProbation_ShouldReturnFalseWhenMoreThan180Days()
        {
            // Arrange
            var staff = new OtherStaff
            {
                JoiningDate = DateTime.UtcNow.AddDays(-200)
            };

            // Act
            var isOnProbation = staff.IsOnProbation;

            // Assert
            Assert.False(isOnProbation);
        }

        [Fact]
        public void IsOnProbation_ShouldReturnFalseWhenJoiningDateIsNull()
        {
            // Arrange
            var staff = new OtherStaff
            {
                JoiningDate = null
            };

            // Act
            var isOnProbation = staff.IsOnProbation;

            // Assert
            Assert.False(isOnProbation);
        }

        [Fact]
        public void Salary_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedSalary = 50000m;

            // Act
            staff.Salary = expectedSalary;

            // Assert
            Assert.Equal(expectedSalary, staff.Salary);
        }

        [Fact]
        public void Email_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedEmail = "jane.smith@hospital.com";

            // Act
            staff.Email = expectedEmail;

            // Assert
            Assert.Equal(expectedEmail, staff.Email);
        }

        [Fact]
        public void Department_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedDepartment = "Emergency";

            // Act
            staff.Department = expectedDepartment;

            // Assert
            Assert.Equal(expectedDepartment, staff.Department);
        }

        [Fact]
        public void EmployeeID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedEmployeeID = "EMP12345";

            // Act
            staff.EmployeeID = expectedEmployeeID;

            // Assert
            Assert.Equal(expectedEmployeeID, staff.EmployeeID);
        }

        [Fact]
        public void TerminationDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedTerminationDate = DateTime.UtcNow;

            // Act
            staff.TerminationDate = expectedTerminationDate;

            // Assert
            Assert.Equal(expectedTerminationDate, staff.TerminationDate);
        }

        [Fact]
        public void TerminationReason_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedReason = "Resignation";

            // Act
            staff.TerminationReason = expectedReason;

            // Assert
            Assert.Equal(expectedReason, staff.TerminationReason);
        }

        [Fact]
        public void CreatedDate_ShouldBeInitialized()
        {
            // Arrange & Act
            var staff = new OtherStaff();

            // Assert
            Assert.True(staff.CreatedDate <= DateTime.UtcNow);
            Assert.True(staff.CreatedDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void IsActive_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();

            // Act
            staff.IsActive = false;

            // Assert
            Assert.False(staff.IsActive);
        }

        [Fact]
        public void Shift_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedShift = "Night";

            // Act
            staff.Shift = expectedShift;

            // Assert
            Assert.Equal(expectedShift, staff.Shift);
        }

        [Fact]
        public void EmploymentType_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedType = "Full-time";

            // Act
            staff.EmploymentType = expectedType;

            // Assert
            Assert.Equal(expectedType, staff.EmploymentType);
        }

        [Fact]
        public void BloodGroup_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var staff = new OtherStaff();
            var expectedBloodGroup = "O+";

            // Act
            staff.BloodGroup = expectedBloodGroup;

            // Assert
            Assert.Equal(expectedBloodGroup, staff.BloodGroup);
        }
    }
}
