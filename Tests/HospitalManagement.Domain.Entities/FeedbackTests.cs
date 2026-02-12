using System;
using System.Linq;
using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests
{
    public class FeedbackTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var feedback = new Feedback();

            // Assert
            Assert.Equal(0, feedback.FeedbackID);
            Assert.Equal(0, feedback.AppointID);
            Assert.Equal(0, feedback.PatientID);
            Assert.Equal(0, feedback.DoctorID);
            Assert.Equal(2, feedback.Status);
            Assert.False(feedback.IsAnonymous);
            Assert.False(feedback.IsReviewed);
            Assert.True(feedback.IsActive);
        }

        [Fact]
        public void FeedbackID_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedId = 111;

            // Act
            feedback.FeedbackID = expectedId;

            // Assert
            Assert.Equal(expectedId, feedback.FeedbackID);
        }

        [Fact]
        public void Rating_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedRating = 5;

            // Act
            feedback.Rating = expectedRating;

            // Assert
            Assert.Equal(expectedRating, feedback.Rating);
        }

        [Fact]
        public void Comments_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedComments = "Excellent service";

            // Act
            feedback.Comments = expectedComments;

            // Assert
            Assert.Equal(expectedComments, feedback.Comments);
        }

        [Fact]
        public void Status_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedStatus = 1;

            // Act
            feedback.Status = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, feedback.Status);
        }

        [Fact]
        public void IsGiven_ShouldReturnTrueWhenStatusIs1()
        {
            // Arrange
            var feedback = new Feedback
            {
                Status = 1
            };

            // Act
            var isGiven = feedback.IsGiven;

            // Assert
            Assert.True(isGiven);
        }

        [Fact]
        public void IsPending_ShouldReturnTrueWhenStatusIs2()
        {
            // Arrange
            var feedback = new Feedback
            {
                Status = 2
            };

            // Act
            var isPending = feedback.IsPending;

            // Assert
            Assert.True(isPending);
        }

        [Fact]
        public void GetAverageRating_ShouldCalculateCorrectly()
        {
            // Arrange
            var feedback = new Feedback
            {
                Rating = 5,
                ProfessionalismRating = 4,
                CommunicationRating = 5,
                TreatmentEffectivenessRating = 4,
                WaitingTimeRating = 3,
                FacilityRating = 4
            };

            // Act
            var averageRating = feedback.GetAverageRating();

            // Assert
            Assert.NotNull(averageRating);
            Assert.True(averageRating >= 4 && averageRating <= 5);
        }

        [Fact]
        public void GetAverageRating_ShouldReturnNullWhenNoRatings()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            var averageRating = feedback.GetAverageRating();

            // Assert
            Assert.Null(averageRating);
        }

        [Fact]
        public void IsPositive_ShouldReturnTrueWhenRatingIsGreaterOrEqualTo4()
        {
            // Arrange
            var feedback = new Feedback
            {
                Rating = 4
            };

            // Act
            var isPositive = feedback.IsPositive;

            // Assert
            Assert.True(isPositive);
        }

        [Fact]
        public void IsPositive_ShouldReturnFalseWhenRatingIsLessThan4()
        {
            // Arrange
            var feedback = new Feedback
            {
                Rating = 3
            };

            // Act
            var isPositive = feedback.IsPositive;

            // Assert
            Assert.False(isPositive);
        }

        [Fact]
        public void IsNegative_ShouldReturnTrueWhenRatingIsLessOrEqualTo2()
        {
            // Arrange
            var feedback = new Feedback
            {
                Rating = 2
            };

            // Act
            var isNegative = feedback.IsNegative;

            // Assert
            Assert.True(isNegative);
        }

        [Fact]
        public void IsNegative_ShouldReturnFalseWhenRatingIsGreaterThan2()
        {
            // Arrange
            var feedback = new Feedback
            {
                Rating = 3
            };

            // Act
            var isNegative = feedback.IsNegative;

            // Assert
            Assert.False(isNegative);
        }

        [Fact]
        public void WouldRecommend_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            feedback.WouldRecommend = true;

            // Assert
            Assert.True(feedback.WouldRecommend);
        }

        [Fact]
        public void ImprovementSuggestions_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedSuggestions = "Reduce waiting time";

            // Act
            feedback.ImprovementSuggestions = expectedSuggestions;

            // Assert
            Assert.Equal(expectedSuggestions, feedback.ImprovementSuggestions);
        }

        [Fact]
        public void PositiveAspects_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedAspects = "Very professional";

            // Act
            feedback.PositiveAspects = expectedAspects;

            // Assert
            Assert.Equal(expectedAspects, feedback.PositiveAspects);
        }

        [Fact]
        public void IsAnonymous_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            feedback.IsAnonymous = true;

            // Assert
            Assert.True(feedback.IsAnonymous);
        }

        [Fact]
        public void IsReviewed_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            feedback.IsReviewed = true;

            // Assert
            Assert.True(feedback.IsReviewed);
        }

        [Fact]
        public void ReviewedDate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedDate = DateTime.UtcNow;

            // Act
            feedback.ReviewedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, feedback.ReviewedDate);
        }

        [Fact]
        public void ReviewedBy_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedReviewer = "admin";

            // Act
            feedback.ReviewedBy = expectedReviewer;

            // Assert
            Assert.Equal(expectedReviewer, feedback.ReviewedBy);
        }

        [Fact]
        public void FeedbackDate_ShouldBeInitialized()
        {
            // Arrange & Act
            var feedback = new Feedback();

            // Assert
            Assert.True(feedback.FeedbackDate <= DateTime.UtcNow);
            Assert.True(feedback.FeedbackDate > DateTime.UtcNow.AddMinutes(-1));
        }

        [Fact]
        public void ProfessionalismRating_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedRating = 5;

            // Act
            feedback.ProfessionalismRating = expectedRating;

            // Assert
            Assert.Equal(expectedRating, feedback.ProfessionalismRating);
        }

        [Fact]
        public void CommunicationRating_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedRating = 4;

            // Act
            feedback.CommunicationRating = expectedRating;

            // Assert
            Assert.Equal(expectedRating, feedback.CommunicationRating);
        }

        [Fact]
        public void TreatmentEffectivenessRating_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var feedback = new Feedback();
            var expectedRating = 5;

            // Act
            feedback.TreatmentEffectivenessRating = expectedRating;

            // Assert
            Assert.Equal(expectedRating, feedback.TreatmentEffectivenessRating);
        }
    }
}
