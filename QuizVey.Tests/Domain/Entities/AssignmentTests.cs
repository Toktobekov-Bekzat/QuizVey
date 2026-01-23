using System;
using QuizVey.Domain.Entities;
using QuizVey.Domain.Enums;
using Xunit;

namespace QuizVey.Tests.Domain.Entities
{
    public class AssignmentTests
    {
        [Fact]
        public void Assignment_ShouldNotAllow_SecondInProgressAttempt()
        {
            // ---------- Arrange ----------
            var userId = Guid.NewGuid();

            // Assessment of type QUIZ
            var assessment = new Assessment(
                title: "Sample Quiz",
                type: AssessmentType.Quiz,
                description: null
            );

            // First version was auto-created by Assessment constructor
            var version = assessment.Versions.First();

            var assignment = new Assignment(userId, version.Id);

            // ---------- Act ----------
            assignment.StartAttempt(assessment, version);

            // ---------- Assert ----------
            Assert.Throws<InvalidOperationException>(() =>
                assignment.StartAttempt(assessment, version)
            );
        }
    }
}
