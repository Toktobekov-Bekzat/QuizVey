using System;
using System.Collections.Generic;
using System.Linq;
using QuizVey.Domain.Entities;
using QuizVey.Domain.Enums;
using Xunit;

namespace QuizVey.Tests.Domain.Entities
{
    public class AssessmentVersionTests
    {
        [Fact]
        public void EvaluateQuiz_PartialAnswers_ShouldPass_WhenThresholdMet()
        {
            // ---------- Arrange ----------
            var version = CreateVersionWithPassingThreshold(50);

            var q1 = CreateQuestion("Q1", "A");
            var q2 = CreateQuestion("Q2", "B");
            var q3 = CreateQuestion("Q3", "C");
            var q4 = CreateQuestion("Q4", "D");
            var q5 = CreateQuestion("Q5", "E");

            version.AddQuestion(q1);
            version.AddQuestion(q2);
            version.AddQuestion(q3);
            version.AddQuestion(q4);
            version.AddQuestion(q5);

            // 3 out of 5 correct = 60% → should pass 50% threshold
            var answers = new List<AttemptAnswer>
            {
                new AttemptAnswer(Guid.NewGuid(), q1.Id, "A"),
                new AttemptAnswer(Guid.NewGuid(), q2.Id, "B"),
                new AttemptAnswer(Guid.NewGuid(), q3.Id, "C")
            };

            // ---------- Act ----------
            var result = version.EvaluateQuiz(answers);

            // ---------- Assert ----------
            Assert.True(result);
        }


        // ----------------------------------------------------------
        // Helpers
        // ----------------------------------------------------------

        private static AssessmentVersion CreateVersionWithPassingThreshold(int passingPercentage)
        {
            var version = new AssessmentVersion(
                Guid.NewGuid(),    // assessmentId
                1,                 // versionNumber
                retakesAllowed: false,
                maxAttempts: null
            );

            typeof(AssessmentVersion)
                .GetProperty("PassingPercentage")!
                .SetValue(version, passingPercentage);

            return version;
        }

        private static Question CreateQuestion(string text, string correctAnswer)
        {
            var q = new Question(text, QuestionType.SingleChoice);

            // Apply options + correct answers
            q.Update(
                text,
                description: null,
                type: QuestionType.SingleChoice,
                options: new[] { correctAnswer },
                correctAnswers: new[] { correctAnswer }
            );

            return q;
        }
    }
}
