using System;
using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests
{
    public class TreatmentHistoryTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var treatmentHistory = new TreatmentHistory();

            // Assert
            Assert.Equal(0, treatmentHistory.TreatmentHistoryID);
            Assert.Equal(0, treatmentHistory.AppointID);
            Assert.Equal(0, treatmentHistory.PatientID);
            Assert.Equal(0, treatmentHistory.DoctorID);
            Assert.True(treatmentHistory.IsActive);
        }

        [Fact]
        public void TreatmentHistoryID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedId = 789;

            // Act
            treatmentHistory.TreatmentHistoryID = expectedId;

            // Assert
            Assert.Equal(expectedId, treatmentHistory.TreatmentHistoryID);
        }

        [Fact]
        public void AppointID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedAppointID = 123;

            // Act
            treatmentHistory.AppointID = expectedAppointID;

            // Assert
            Assert.Equal(expectedAppointID, treatmentHistory.AppointID);
        }

        [Fact]
        public void PatientID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedPatientID = 456;

            // Act
            treatmentHistory.PatientID = expectedPatientID;

            // Assert
            Assert.Equal(expectedPatientID, treatmentHistory.PatientID);
        }

        [Fact]
        public void DoctorID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedDoctorID = 789;

            // Act
            treatmentHistory.DoctorID = expectedDoctorID;

            // Assert
            Assert.Equal(expectedDoctorID, treatmentHistory.DoctorID);
        }

        [Fact]
        public void TreatmentDate_ShouldBeInitialized()
        {
            // Arrange & Act
            var treatmentHistory = new TreatmentHistory();

            // Assert
            Assert.True(treatmentHistory.TreatmentDate <= DateTime.UtcNow);
            Assert.True(treatmentHistory.TreatmentDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void Disease_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedDisease = "Flu";

            // Act
            treatmentHistory.Disease = expectedDisease;

            // Assert
            Assert.Equal(expectedDisease, treatmentHistory.Disease);
        }

        [Fact]
        public void Symptoms_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedSymptoms = "Fever, cough";

            // Act
            treatmentHistory.Symptoms = expectedSymptoms;

            // Assert
            Assert.Equal(expectedSymptoms, treatmentHistory.Symptoms);
        }

        [Fact]
        public void Diagnosis_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedDiagnosis = "Influenza type A";

            // Act
            treatmentHistory.Diagnosis = expectedDiagnosis;

            // Assert
            Assert.Equal(expectedDiagnosis, treatmentHistory.Diagnosis);
        }

        [Fact]
        public void TreatmentPlan_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedPlan = "Rest and medication";

            // Act
            treatmentHistory.TreatmentPlan = expectedPlan;

            // Assert
            Assert.Equal(expectedPlan, treatmentHistory.TreatmentPlan);
        }

        [Fact]
        public void Prescription_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedPrescription = "Paracetamol 500mg";

            // Act
            treatmentHistory.Prescription = expectedPrescription;

            // Assert
            Assert.Equal(expectedPrescription, treatmentHistory.Prescription);
        }

        [Fact]
        public void Progress_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedProgress = "Improving";

            // Act
            treatmentHistory.Progress = expectedProgress;

            // Assert
            Assert.Equal(expectedProgress, treatmentHistory.Progress);
        }

        [Fact]
        public void NextFollowUpDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedDate = DateTime.UtcNow.AddDays(7);

            // Act
            treatmentHistory.NextFollowUpDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, treatmentHistory.NextFollowUpDate);
        }

        [Fact]
        public void IsFollowUpDue_ShouldReturnTrueWhenFollowUpDateIsPast()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory
            {
                NextFollowUpDate = DateTime.UtcNow.AddDays(-1)
            };

            // Act
            var isFollowUpDue = treatmentHistory.IsFollowUpDue;

            // Assert
            Assert.True(isFollowUpDue);
        }

        [Fact]
        public void IsFollowUpDue_ShouldReturnFalseWhenFollowUpDateIsFuture()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory
            {
                NextFollowUpDate = DateTime.UtcNow.AddDays(7)
            };

            // Act
            var isFollowUpDue = treatmentHistory.IsFollowUpDue;

            // Assert
            Assert.False(isFollowUpDue);
        }

        [Fact]
        public void IsFollowUpDue_ShouldReturnFalseWhenNextFollowUpDateIsNull()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory
            {
                NextFollowUpDate = null
            };

            // Act
            var isFollowUpDue = treatmentHistory.IsFollowUpDue;

            // Assert
            Assert.False(isFollowUpDue);
        }

        [Fact]
        public void DaysSinceTreatment_ShouldCalculateCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory
            {
                TreatmentDate = DateTime.UtcNow.AddDays(-5)
            };

            // Act
            var daysSinceTreatment = treatmentHistory.DaysSinceTreatment;

            // Assert
            Assert.True(daysSinceTreatment >= 4 && daysSinceTreatment <= 5);
        }

        [Fact]
        public void LabResults_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedLabResults = "Blood test: normal";

            // Act
            treatmentHistory.LabResults = expectedLabResults;

            // Assert
            Assert.Equal(expectedLabResults, treatmentHistory.LabResults);
        }

        [Fact]
        public void VitalSigns_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedVitalSigns = "BP: 120/80, Temp: 98.6F";

            // Act
            treatmentHistory.VitalSigns = expectedVitalSigns;

            // Assert
            Assert.Equal(expectedVitalSigns, treatmentHistory.VitalSigns);
        }

        [Fact]
        public void Notes_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedNotes = "Patient is recovering well";

            // Act
            treatmentHistory.Notes = expectedNotes;

            // Assert
            Assert.Equal(expectedNotes, treatmentHistory.Notes);
        }

        [Fact]
        public void IsActive_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();

            // Act
            treatmentHistory.IsActive = false;

            // Assert
            Assert.False(treatmentHistory.IsActive);
        }

        [Fact]
        public void CreatedDate_ShouldBeInitialized()
        {
            // Arrange & Act
            var treatmentHistory = new TreatmentHistory();

            // Assert
            Assert.True(treatmentHistory.CreatedDate <= DateTime.UtcNow);
            Assert.True(treatmentHistory.CreatedDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void FollowUpInstructions_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var treatmentHistory = new TreatmentHistory();
            var expectedInstructions = "Return in 7 days";

            // Act
            treatmentHistory.FollowUpInstructions = expectedInstructions;

            // Assert
            Assert.Equal(expectedInstructions, treatmentHistory.FollowUpInstructions);
        }
    }
}
