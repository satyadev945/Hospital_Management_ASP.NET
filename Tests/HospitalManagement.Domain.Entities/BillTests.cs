using System;
using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests
{
    public class BillTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var bill = new Bill();

            // Assert
            Assert.Equal(0, bill.BillID);
            Assert.Equal(0, bill.AppointID);
            Assert.Equal(0, bill.PatientID);
            Assert.Equal(0, bill.Amount);
            Assert.Equal("Unpaid", bill.Status);
            Assert.True(bill.IsActive);
        }

        [Fact]
        public void BillID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedId = 999;

            // Act
            bill.BillID = expectedId;

            // Assert
            Assert.Equal(expectedId, bill.BillID);
        }

        [Fact]
        public void AppointID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedAppointID = 123;

            // Act
            bill.AppointID = expectedAppointID;

            // Assert
            Assert.Equal(expectedAppointID, bill.AppointID);
        }

        [Fact]
        public void PatientID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedPatientID = 456;

            // Act
            bill.PatientID = expectedPatientID;

            // Assert
            Assert.Equal(expectedPatientID, bill.PatientID);
        }

        [Fact]
        public void Amount_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedAmount = 250.50m;

            // Act
            bill.Amount = expectedAmount;

            // Assert
            Assert.Equal(expectedAmount, bill.Amount);
        }

        [Fact]
        public void Status_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedStatus = "Paid";

            // Act
            bill.Status = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, bill.Status);
        }

        [Fact]
        public void IsPaid_ShouldReturnTrueWhenStatusIsPaid()
        {
            // Arrange
            var bill = new Bill
            {
                Status = "Paid"
            };

            // Act
            var isPaid = bill.IsPaid;

            // Assert
            Assert.True(isPaid);
        }

        [Fact]
        public void IsPaid_ShouldReturnFalseWhenStatusIsUnpaid()
        {
            // Arrange
            var bill = new Bill
            {
                Status = "Unpaid"
            };

            // Act
            var isPaid = bill.IsPaid;

            // Assert
            Assert.False(isPaid);
        }

        [Fact]
        public void IsOverdue_ShouldReturnTrueWhenUnpaidAndOlderThan30Days()
        {
            // Arrange
            var bill = new Bill
            {
                Status = "Unpaid",
                BillDate = DateTime.UtcNow.AddDays(-31)
            };

            // Act
            var isOverdue = bill.IsOverdue;

            // Assert
            Assert.True(isOverdue);
        }

        [Fact]
        public void IsOverdue_ShouldReturnFalseWhenPaid()
        {
            // Arrange
            var bill = new Bill
            {
                Status = "Paid",
                BillDate = DateTime.UtcNow.AddDays(-31)
            };

            // Act
            var isOverdue = bill.IsOverdue;

            // Assert
            Assert.False(isOverdue);
        }

        [Fact]
        public void IsOverdue_ShouldReturnFalseWhenUnpaidButNotOlderThan30Days()
        {
            // Arrange
            var bill = new Bill
            {
                Status = "Unpaid",
                BillDate = DateTime.UtcNow.AddDays(-20)
            };

            // Act
            var isOverdue = bill.IsOverdue;

            // Assert
            Assert.False(isOverdue);
        }

        [Fact]
        public void CalculateTotalAmount_ShouldCalculateCorrectly()
        {
            // Arrange
            var bill = new Bill
            {
                ConsultationCharges = 100m,
                MedicationCharges = 50m,
                LabCharges = 75m,
                OtherCharges = 25m,
                Discount = 20m,
                Tax = 10m
            };

            // Act
            var totalAmount = bill.CalculateTotalAmount();

            // Assert
            Assert.Equal(240m, totalAmount);
        }

        [Fact]
        public void CalculateTotalAmount_ShouldHandleNullCharges()
        {
            // Arrange
            var bill = new Bill
            {
                ConsultationCharges = 100m,
                Discount = 10m,
                Tax = 5m
            };

            // Act
            var totalAmount = bill.CalculateTotalAmount();

            // Assert
            Assert.Equal(95m, totalAmount);
        }

        [Fact]
        public void PaymentDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedDate = DateTime.UtcNow;

            // Act
            bill.PaymentDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, bill.PaymentDate);
        }

        [Fact]
        public void PaymentMethod_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedMethod = "Credit Card";

            // Act
            bill.PaymentMethod = expectedMethod;

            // Assert
            Assert.Equal(expectedMethod, bill.PaymentMethod);
        }

        [Fact]
        public void TransactionReference_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedReference = "TXN123456";

            // Act
            bill.TransactionReference = expectedReference;

            // Assert
            Assert.Equal(expectedReference, bill.TransactionReference);
        }

        [Fact]
        public void Notes_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedNotes = "Partial payment received";

            // Act
            bill.Notes = expectedNotes;

            // Assert
            Assert.Equal(expectedNotes, bill.Notes);
        }

        [Fact]
        public void BillDate_ShouldBeInitialized()
        {
            // Arrange & Act
            var bill = new Bill();

            // Assert
            Assert.True(bill.BillDate <= DateTime.UtcNow);
            Assert.True(bill.BillDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void IsActive_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();

            // Act
            bill.IsActive = false;

            // Assert
            Assert.False(bill.IsActive);
        }

        [Fact]
        public void DoctorID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedDoctorID = 789;

            // Act
            bill.DoctorID = expectedDoctorID;

            // Assert
            Assert.Equal(expectedDoctorID, bill.DoctorID);
        }

        [Fact]
        public void ConsultationCharges_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedCharges = 150.00m;

            // Act
            bill.ConsultationCharges = expectedCharges;

            // Assert
            Assert.Equal(expectedCharges, bill.ConsultationCharges);
        }

        [Fact]
        public void Discount_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var bill = new Bill();
            var expectedDiscount = 25.00m;

            // Act
            bill.Discount = expectedDiscount;

            // Assert
            Assert.Equal(expectedDiscount, bill.Discount);
        }
    }
}
