using System;
using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests
{
    public class DepartmentTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var department = new Department();

            // Assert
            Assert.Equal(0, department.DeptNo);
            Assert.Equal(string.Empty, department.DeptName);
            Assert.True(department.IsActive);
            Assert.NotNull(department.Doctors);
        }

        [Fact]
        public void DeptNo_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var department = new Department();
            var expectedDeptNo = 5;

            // Act
            department.DeptNo = expectedDeptNo;

            // Assert
            Assert.Equal(expectedDeptNo, department.DeptNo);
        }

        [Fact]
        public void DeptName_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var department = new Department();
            var expectedDeptName = "Cardiology";

            // Act
            department.DeptName = expectedDeptName;

            // Assert
            Assert.Equal(expectedDeptName, department.DeptName);
        }

        [Fact]
        public void DepartmentName_ShouldGetDeptName()
        {
            // Arrange
            var department = new Department
            {
                DeptName = "Neurology"
            };

            // Act
            var departmentName = department.DepartmentName;

            // Assert
            Assert.Equal("Neurology", departmentName);
        }

        [Fact]
        public void DepartmentName_ShouldSetDeptName()
        {
            // Arrange
            var department = new Department();

            // Act
            department.DepartmentName = "Orthopedics";

            // Assert
            Assert.Equal("Orthopedics", department.DeptName);
        }

        [Fact]
        public void Description_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var department = new Department();
            var expectedDescription = "Department of heart and cardiovascular care";

            // Act
            department.Description = expectedDescription;

            // Assert
            Assert.Equal(expectedDescription, department.Description);
        }

        [Fact]
        public void IsActive_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var department = new Department();

            // Act
            department.IsActive = false;

            // Assert
            Assert.False(department.IsActive);
        }

        [Fact]
        public void CreatedDate_ShouldBeInitialized()
        {
            // Arrange & Act
            var department = new Department();

            // Assert
            Assert.True(department.CreatedDate <= DateTime.UtcNow);
            Assert.True(department.CreatedDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void ModifiedDate_ShouldBeNullByDefault()
        {
            // Arrange & Act
            var department = new Department();

            // Assert
            Assert.Null(department.ModifiedDate);
        }

        [Fact]
        public void ModifiedDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var department = new Department();
            var expectedDate = DateTime.UtcNow;

            // Act
            department.ModifiedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, department.ModifiedDate);
        }

        [Fact]
        public void CreatedBy_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var department = new Department();
            var expectedCreatedBy = "admin";

            // Act
            department.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, department.CreatedBy);
        }

        [Fact]
        public void ModifiedBy_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var department = new Department();
            var expectedModifiedBy = "admin";

            // Act
            department.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, department.ModifiedBy);
        }

        [Fact]
        public void Doctors_ShouldInitializeAsEmptyList()
        {
            // Arrange & Act
            var department = new Department();

            // Assert
            Assert.NotNull(department.Doctors);
            Assert.Empty(department.Doctors);
        }

        [Fact]
        public void Description_CanBeNull()
        {
            // Arrange
            var department = new Department();

            // Act
            department.Description = null;

            // Assert
            Assert.Null(department.Description);
        }

        [Fact]
        public void IsActive_DefaultsToTrue()
        {
            // Arrange & Act
            var department = new Department();

            // Assert
            Assert.True(department.IsActive);
        }
    }
}
