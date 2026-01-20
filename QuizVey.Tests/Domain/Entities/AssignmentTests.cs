using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var userId = Guid.NewGuid();
            var assessmentVersionId = Guid.NewGuid();

            var assignment = new Assignment(userId, assessmentVersionId);

            var version = CreateQuizVersion();

            assignment.StartAttempt(version);

            Assert.Throws<InvalidOperationException>(() =>
                assignment.StartAttempt(version)
            );
        }
        
        private static AssessmentVersion CreateQuizVersion()
        {
            var version = new AssessmentVersion(
                Guid.NewGuid(),
                versionNumber: 1,
                type: AssessmentType.Quiz,
                retakesAllowed: true
            );

            return version;
        }
    }
}