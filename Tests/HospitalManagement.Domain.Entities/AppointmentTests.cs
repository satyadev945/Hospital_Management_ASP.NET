using System;
using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests
{
    public class AppointmentTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var appointment = new Appointment();

            // Assert
            Assert.Equal(0, appointment.AppointID);
            Assert.True(appointment.IsActive);
        }

        [Fact]
        public void AppointID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedId = 123;

            // Act
            appointment.AppointID = expectedId;

            // Assert
            Assert.Equal(expectedId, appointment.AppointID);
        }

        [Fact]
        public void DoctorID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedDoctorID = 456;

            // Act
            appointment.DoctorID = expectedDoctorID;

            // Assert
            Assert.Equal(expectedDoctorID, appointment.DoctorID);
        }

        [Fact]
        public void PatientID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedPatientID = 789;

            // Act
            appointment.PatientID = expectedPatientID;

            // Assert
            Assert.Equal(expectedPatientID, appointment.PatientID);
        }

        [Fact]
        public void Date_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedDate = DateTime.UtcNow.AddDays(1);

            // Act
            appointment.Date = expectedDate;

            // Assert
            Assert.Equal(expectedDate, appointment.Date);
        }

        [Fact]
        public void AppointmentStatus_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedStatus = 1;

            // Act
            appointment.AppointmentStatus = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, appointment.AppointmentStatus);
        }

        [Fact]
        public void IsPending_ShouldReturnTrueWhenStatusIs2()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentStatus = 2
            };

            // Act
            var isPending = appointment.IsPending;

            // Assert
            Assert.True(isPending);
        }

        [Fact]
        public void IsApproved_ShouldReturnTrueWhenStatusIs1()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentStatus = 1
            };

            // Act
            var isApproved = appointment.IsApproved;

            // Assert
            Assert.True(isApproved);
        }

        [Fact]
        public void IsCompleted_ShouldReturnTrueWhenStatusIs3()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentStatus = 3
            };

            // Act
            var isCompleted = appointment.IsCompleted;

            // Assert
            Assert.True(isCompleted);
        }

        [Fact]
        public void IsRejected_ShouldReturnTrueWhenStatusIs4()
        {
            // Arrange
            var appointment = new Appointment
            {
                AppointmentStatus = 4
            };

            // Act
            var isRejected = appointment.IsRejected;

            // Assert
            Assert.True(isRejected);
        }

        [Fact]
        public void BillAmount_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedAmount = 150.00m;

            // Act
            appointment.BillAmount = expectedAmount;

            // Assert
            Assert.Equal(expectedAmount, appointment.BillAmount);
        }

        [Fact]
        public void BillStatus_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedStatus = "Paid";

            // Act
            appointment.BillStatus = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, appointment.BillStatus);
        }

        [Fact]
        public void IsBillPaid_ShouldReturnTrueWhenBillStatusIsPaid()
        {
            // Arrange
            var appointment = new Appointment
            {
                BillStatus = "Paid"
            };

            // Act
            var isBillPaid = appointment.IsBillPaid;

            // Assert
            Assert.True(isBillPaid);
        }

        [Fact]
        public void IsBillPaid_ShouldReturnFalseWhenBillStatusIsUnpaid()
        {
            // Arrange
            var appointment = new Appointment
            {
                BillStatus = "Unpaid"
            };

            // Act
            var isBillPaid = appointment.IsBillPaid;

            // Assert
            Assert.False(isBillPaid);
        }

        [Fact]
        public void DoctorNotification_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedNotification = 1;

            // Act
            appointment.DoctorNotification = expectedNotification;

            // Assert
            Assert.Equal(expectedNotification, appointment.DoctorNotification);
        }

        [Fact]
        public void PatientNotification_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedNotification = 2;

            // Act
            appointment.PatientNotification = expectedNotification;

            // Assert
            Assert.Equal(expectedNotification, appointment.PatientNotification);
        }

        [Fact]
        public void FeedbackStatus_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedStatus = 1;

            // Act
            appointment.FeedbackStatus = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, appointment.FeedbackStatus);
        }

        [Fact]
        public void Disease_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedDisease = "Common Cold";

            // Act
            appointment.Disease = expectedDisease;

            // Assert
            Assert.Equal(expectedDisease, appointment.Disease);
        }

        [Fact]
        public void Progress_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedProgress = "Recovering well";

            // Act
            appointment.Progress = expectedProgress;

            // Assert
            Assert.Equal(expectedProgress, appointment.Progress);
        }

        [Fact]
        public void Prescription_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedPrescription = "Aspirin 500mg";

            // Act
            appointment.Prescription = expectedPrescription;

            // Assert
            Assert.Equal(expectedPrescription, appointment.Prescription);
        }

        [Fact]
        public void IsActive_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();

            // Act
            appointment.IsActive = false;

            // Assert
            Assert.False(appointment.IsActive);
        }

        [Fact]
        public void CreatedDate_ShouldBeInitialized()
        {
            // Arrange & Act
            var appointment = new Appointment();

            // Assert
            Assert.True(appointment.CreatedDate <= DateTime.UtcNow);
            Assert.True(appointment.CreatedDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void ModifiedDate_ShouldBeNullByDefault()
        {
            // Arrange & Act
            var appointment = new Appointment();

            // Assert
            Assert.Null(appointment.ModifiedDate);
        }

        [Fact]
        public void ModifiedDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var appointment = new Appointment();
            var expectedDate = DateTime.UtcNow;

            // Act
            appointment.ModifiedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, appointment.ModifiedDate);
        }
    }
}
