using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Domain.Enums;

namespace QuizVey.Domain.Entities
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

            int nextOrder = _questions.Count == 0 ? 1 : _questions.Max(q => q.Order) + 1;
            question.SetOrder(nextOrder);
            _questions.Add(question);
        }

        public void RemoveQuestion(Guid questionId)
        {
            if (Status != AssessmentVersionStatus.Draft)
                throw new InvalidOperationException(
                    "Questions can only be removed when the version is in Draft."
                );

            var question = _questions.SingleOrDefault(q => q.Id == questionId);

            if (question == null)
                throw new InvalidOperationException("Question not found in this version.");

            // Remove the question
            _questions.Remove(question);

            // Reorder remaining questions: 1,2,3,4,...
            int order = 1;
            foreach (var q in _questions.OrderBy(q => q.Order))
            {
                q.SetOrder(order++);
            }
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

        public void SetPassingPercentage(int percentage)
        {
            if (Status != AssessmentVersionStatus.Draft)
                throw new InvalidOperationException(
                    "Passing percentage can only be changed in Draft."
                );

            if (percentage is < 0 or > 100)
                throw new InvalidOperationException(
                    "Passing percentage must be between 0 and 100."
                );

            PassingPercentage = percentage;
        }

        public void ConfigureAttempts(bool retakesAllowed, int? maxAttempts)
        {
            if (Status != AssessmentVersionStatus.Draft)
                throw new InvalidOperationException(
                    "Number of attempts can only be configured in draft versions."
                );

            if (Type == AssessmentType.Survey)
                throw new InvalidOperationException(
                    "Surveys cannot allow retakes."
                );

            if (!retakesAllowed)
            {
                // No retakes → maxAttempts must be null OR 1
                if (maxAttempts.HasValue && maxAttempts.Value != 1)
                    throw new InvalidOperationException(
                        "When retakes are disabled, MaxAttempts must be 1 or null."
                    );

                RetakesAllowed = false;
                MaxAttempts = maxAttempts ?? 1;
                return;
            }

            // Retakes allowed → maxAttempts REQUIRED
            if (!maxAttempts.HasValue)
                throw new InvalidOperationException(
                    "MaxAttempts must be specified when retakes are allowed."
                );

            if (maxAttempts.Value < 1)
                throw new InvalidOperationException(
                    "MaxAttempts must be at least 1."
                );

            RetakesAllowed = true;
            MaxAttempts = maxAttempts.Value;
        }

        public void ReorderQuestions(List<Guid> orderedIds)
        {
            if (Status != AssessmentVersionStatus.Draft)
                throw new InvalidOperationException("Reorder only allowed in Draft.");

            if (orderedIds.Count != _questions.Count)
                throw new InvalidOperationException("Invalid question count.");

            var dict = _questions.ToDictionary(q => q.Id);

            for (int i = 0; i < orderedIds.Count; i++)
            {
                var q = dict[orderedIds[i]];
                q.SetOrder(i + 1);
            }

            _questions.Sort((a, b) => a.Order.CompareTo(b.Order));
        }

        public Question UpdateQuestion(
            Guid questionId,
            string text,
            string? description,
            QuestionType type,
            IEnumerable<string> options,
            IEnumerable<string> correctAnswers)
        {
        if (Status != AssessmentVersionStatus.Draft)
            throw new InvalidOperationException("Only draft versions can be edited.");

        var question = _questions.SingleOrDefault(q => q.Id == questionId)
            ?? throw new InvalidOperationException("Question not found.");

        question.Update(text, description, type, options, correctAnswers);

        return question;
        }




    }
}