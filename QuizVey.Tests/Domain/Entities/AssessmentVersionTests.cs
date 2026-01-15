using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Api.Domain.Entities;
using QuizVey.Api.Domain.Enums;
using Xunit;

namespace QuizVey.Tests.Domain.Entities
{
    public class AssessmentVersionTests
    {
        [Fact]
        public void Evaluate_PartialAnswers_ShouldPass_WhenThresholdMet()
        {
            // ---------- Arrange ----------
            var version = CreateQuizVersion(passingPercentage: 50);

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

            var answers = new List<AttemptAnswer>
            {
                new AttemptAnswer(Guid.NewGuid(), q1.Id, "A"),
                new AttemptAnswer(Guid.NewGuid(), q2.Id, "B"),
                new AttemptAnswer(Guid.NewGuid(), q3.Id, "C")
            };

            // ---------- Act ----------
            var result = version.Evaluate(answers);

            // ---------- Assert ----------
            Assert.True(result);
        }

        private static AssessmentVersion CreateQuizVersion(int passingPercentage)
        {
            var version = new AssessmentVersion(
                Guid.NewGuid(),
                1,
                AssessmentType.Quiz
            );

            typeof(AssessmentVersion)
                .GetProperty("PassingPercentage")!
                .SetValue(version, passingPercentage);

            return version;
        }

        private static Question CreateQuestion(string text, string correctAnswer)
        {
            var question = new Question(text, QuestionType.SingleChoice);
            question.SetCorrectAnswers(new[] { correctAnswer });
            return question;
        }
        
    }
}