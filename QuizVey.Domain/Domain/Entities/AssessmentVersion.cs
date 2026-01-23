using System;
using System.Collections.Generic;
using System.Linq;
using QuizVey.Domain.Enums;

namespace QuizVey.Domain.Entities
{
    public class AssessmentVersion : BaseEntity
    {
        public Guid AssessmentId { get; private set; }
        public int VersionNumber { get; private set; }

        public bool RetakesAllowed { get; private set; }
        public int? MaxAttempts { get; private set; }
        public int PassingPercentage { get; private set; } = 50;

        public AssessmentVersionStatus Status { get; private set; } =
            AssessmentVersionStatus.Draft;

        private readonly List<Question> _questions = new();
        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

        protected AssessmentVersion() { }

        internal AssessmentVersion(
            Guid assessmentId,
            int versionNumber,
            bool retakesAllowed = false,
            int? maxAttempts = null)
        {
            AssessmentId = assessmentId;
            VersionNumber = versionNumber;
            RetakesAllowed = retakesAllowed;
            MaxAttempts = maxAttempts;
        }

        // ---------------------------------------------------------
        // QUESTIONS
        // ---------------------------------------------------------

        public void AddQuestion(Question question)
        {
            EnsureDraft();

            if (question == null)
                throw new ArgumentNullException(nameof(question));

            int nextOrder = _questions.Count == 0
                ? 1
                : _questions.Max(q => q.Order) + 1;

            question.SetOrder(nextOrder);
            _questions.Add(question);
        }

        public void RemoveQuestion(Guid questionId)
        {
            EnsureDraft();

            var question = _questions.SingleOrDefault(q => q.Id == questionId)
                ?? throw new InvalidOperationException("Question not found.");

            _questions.Remove(question);

            // Reorder 1..N
            int order = 1;
            foreach (var q in _questions.OrderBy(q => q.Order))
                q.SetOrder(order++);
        }

        public Question UpdateQuestion(
            Guid questionId,
            string text,
            string? description,
            QuestionType type,
            IEnumerable<string> options,
            IEnumerable<string> correctAnswers)
        {
            EnsureDraft();

            var question = _questions.SingleOrDefault(q => q.Id == questionId)
                ?? throw new InvalidOperationException("Question not found.");

            question.Update(text, description, type, options, correctAnswers);

            return question;
        }

        public void ReorderQuestions(List<Guid> orderedIds)
        {
            EnsureDraft();

            if (orderedIds.Count != _questions.Count)
                throw new InvalidOperationException("Invalid question count.");

            var lookup = _questions.ToDictionary(q => q.Id);

            for (int i = 0; i < orderedIds.Count; i++)
                lookup[orderedIds[i]].SetOrder(i + 1);

            _questions.Sort((a, b) => a.Order.CompareTo(b.Order));
        }

        // ---------------------------------------------------------
        // VERSION MANAGEMENT
        // ---------------------------------------------------------

        public void Activate()
        {
            if (Status != AssessmentVersionStatus.Draft)
                throw new InvalidOperationException("Only draft versions can be activated.");

            Status = AssessmentVersionStatus.Active;
        }

        public void Archive()
        {
            if (Status != AssessmentVersionStatus.Active)
                throw new InvalidOperationException("Only active versions can be archived.");

            Status = AssessmentVersionStatus.Archived;
        }

        internal AssessmentVersion CloneAsNext(int nextVersionNumber)
        {
            var clone = new AssessmentVersion(
                AssessmentId,
                nextVersionNumber,
                RetakesAllowed,
                MaxAttempts
            );

            foreach (var question in _questions.OrderBy(q => q.Order))
            {
                var qClone = question.Clone();
                qClone.SetOrder(question.Order);
                clone.AddClonedQuestion(qClone);
            }

            return clone;
        }

        // ---------------------------------------------------------
        // VALIDATION HELPERS
        // ---------------------------------------------------------

        private void EnsureDraft()
        {
            if (Status != AssessmentVersionStatus.Draft)
                throw new InvalidOperationException("Only draft versions can be modified.");
        }

        internal void AddClonedQuestion(Question question)
        {
            _questions.Add(question);
        }

        // ---------------------------------------------------------
        // EVALUATION FOR QUIZZES (caller must check assessment type)
        // ---------------------------------------------------------

        public bool EvaluateQuiz(IReadOnlyCollection<AttemptAnswer> answers)
        {
            var evaluatable = _questions
                .Where(q => q.Type != QuestionType.FreeText)
                .ToList();

            if (!evaluatable.Any())
                throw new InvalidOperationException(
                    "Quiz contains no evaluatable questions."
                );

            var answerLookup = answers?
                .GroupBy(a => a.QuestionId)
                .ToDictionary(g => g.Key, g => g.First())
                ?? new Dictionary<Guid, AttemptAnswer>();

            int correct = 0;

            foreach (var question in evaluatable)
            {
                if (!answerLookup.TryGetValue(question.Id, out var submitted))
                    continue;

                if (question.IsCorrect(submitted.Value))
                    correct++;
            }

            double score = correct * 100.0 / evaluatable.Count;

            return score >= PassingPercentage;
        }
    }
}
