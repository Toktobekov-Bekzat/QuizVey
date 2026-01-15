using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Api.Domain.Enums;

namespace QuizVey.Api.Domain.Entities
{
    public class AssessmentVersion : BaseEntity
    {
        public Guid AssessmentId { get; private set; }
        public int VersionNumber { get; private set; }
        public AssessmentType Type { get; set; }

        // Quiz-only configuration
        public bool RetakesAllowed { get; private set; }
        public int? MaxAttempts { get; private set; }
        public int PassingPercentage { get; private set; } = 50;
        public AssessmentVersionStatus Status { get; private set;} = AssessmentVersionStatus.Draft;

        private readonly List<Question> _questions = new();

        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

        protected AssessmentVersion(){}

        internal AssessmentVersion(
            Guid assessmentId, 
            int versionNumber,
            AssessmentType type,
            bool retakesAllowed = false,
            int? maxAttempts = null)
        {
            AssessmentId = assessmentId;
            VersionNumber = versionNumber;
            Type = type;
            RetakesAllowed = retakesAllowed;
            MaxAttempts = maxAttempts;
        }

        public void AddQuestion(Question question)
        {
            if (Status != AssessmentVersionStatus.Draft)
                throw new InvalidOperationException(
                    "Questions can only be added to draft versions."
                );

            if (question == null)
                throw new ArgumentNullException(nameof(question));

            _questions.Add(question);
        }

        public AssessmentVersion CloneAsDraft()
        {
            if (Status != AssessmentVersionStatus.Active) throw new InvalidOperationException("Only active version can be cloned.");
            
            var clone = new AssessmentVersion(AssessmentId, VersionNumber + 1, Type, RetakesAllowed, MaxAttempts);

            foreach (var question in _questions)
            {
                clone._questions.Add(question.Clone());
            }

            return clone;
        }

        public void Activate()
        {
            if (Status != AssessmentVersionStatus.Draft) throw new InvalidOperationException("Only draft versions can be activated.");

            Status = AssessmentVersionStatus.Active;
        }

        public void Archive()
        {
            if (Status != AssessmentVersionStatus.Active) throw new InvalidOperationException("Only active versions can be archived.");

            Status = AssessmentVersionStatus.Archived;
        }

        public bool Evaluate(IReadOnlyCollection<AttemptAnswer> answers)
        {
            if (Type != AssessmentType.Quiz)
                throw new InvalidOperationException(
                    "Only quizzes can be evaluated."
                );

            if (PassingPercentage is < 0 or > 100)
                throw new InvalidOperationException(
                    "Invalid passing percentage configuration."
                );

            var evaluatableQuestions = _questions
                .Where(q => q.Type != QuestionType.FreeText)
                .ToList();

            if (!evaluatableQuestions.Any())
                throw new InvalidOperationException(
                    "Quiz contains no evaluatable questions."
                );

            var answerLookup = answers?
                .GroupBy(a => a.QuestionId)
                .ToDictionary(g => g.Key, g => g.First())
                ?? new Dictionary<Guid, AttemptAnswer>();

            int correctAnswers = 0;

            foreach (var question in evaluatableQuestions)
            {
                if (!answerLookup.TryGetValue(question.Id, out var answer))
                    continue; // unanswered → incorrect

                if (question.IsCorrect(answer.Value))
                    correctAnswers++;
            }

            var scorePercentage =
                (double) correctAnswers / evaluatableQuestions.Count * 100;

            return scorePercentage >= PassingPercentage;
        }



    }
}