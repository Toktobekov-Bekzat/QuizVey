using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using QuizVey.Domain.Enums;

namespace QuizVey.Domain.Entities
{
    public class Attempt : BaseEntity
    {
        // ─── Identity & ownership ──────────────────────────────
        public Guid UserId { get; private set; }
        public Guid AssessmentVersionId { get; private set; }

        // ─── Lifecycle ─────────────────────────────────────────
        public AttemptStatus Status { get; private set; } = AttemptStatus.NotStarted;
        public DateTime? StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        // ─── Answers ───────────────────────────────────────────
        private readonly List<AttemptAnswer> _finalAnswers = new();
        private readonly List<AttemptDraftAnswer> _draftAnswers = new();

        public IReadOnlyCollection<AttemptAnswer> Answers =>
            _finalAnswers.AsReadOnly();

        public IReadOnlyCollection<AttemptDraftAnswer> DraftAnswers =>
            _draftAnswers.AsReadOnly();

        protected Attempt() {}

        // ✅ Correct constructor
        internal Attempt(Guid userId, Guid assessmentVersionId)
        {
            UserId = userId;
            AssessmentVersionId = assessmentVersionId;
            Start();
        }

        // ─── Derived state ─────────────────────────────────────
        public bool IsInProgress => Status == AttemptStatus.InProgress;

        public bool IsCompleted =>
            Status == AttemptStatus.Completed ||
            Status == AttemptStatus.Passed ||
            Status == AttemptStatus.Failed;

        // ─── Lifecycle behavior ────────────────────────────────
        public void Start()
        {
            if (Status != AttemptStatus.NotStarted)
                throw new InvalidOperationException("Attempt already started.");

            Status = AttemptStatus.InProgress;
            StartedAt = DateTime.UtcNow;
        }

        // ─── Draft answers ─────────────────────────────────────
        public void SaveDraftAnswer(Guid questionId, string value)
        {
            if (!IsInProgress)
                throw new InvalidOperationException(
                    "Draft answers can only be saved while attempt is in progress."
                );

            if (_finalAnswers.Any())
                throw new InvalidOperationException(
                    "Cannot save draft answers after submission."
                );

            var existing = _draftAnswers
                .SingleOrDefault(x => x.QuestionId == questionId);

            if (existing == null)
            {
                _draftAnswers.Add(
                    new AttemptDraftAnswer(Id, questionId, value)
                );
            }
            else
            {
                existing.Update(value);
            }
        }

        // ─── Submission ────────────────────────────────────────
        public IReadOnlyCollection<AttemptAnswer> SubmitDraftAnswers()
        {
            if (!IsInProgress)
                throw new InvalidOperationException(
                    "Attempt must be in progress to submit."
                );

            if (!_draftAnswers.Any())
                throw new InvalidOperationException(
                    "Cannot submit an attempt with no answers."
                );

            if (_finalAnswers.Any())
                throw new InvalidOperationException(
                    "Attempt has already been submitted."
                );

            foreach (var draft in _draftAnswers)
            {
                _finalAnswers.Add(
                    new AttemptAnswer(
                        Id,
                        draft.QuestionId,
                        draft.Value
                    )
                );
            }

            _draftAnswers.Clear();

            return _finalAnswers.AsReadOnly();
        }

        // ─── Completion ────────────────────────────────────────
        public void CompleteSurvey()
        {
            if (!IsInProgress)
                throw new InvalidOperationException(
                    "Survey attempt must be in progress."
                );

            Status = AttemptStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void CompleteQuiz(bool passed)
        {
            if (!IsInProgress)
                throw new InvalidOperationException(
                    "Quiz attempt must be in progress."
                );

            Status = passed
                ? AttemptStatus.Passed
                : AttemptStatus.Failed;

            CompletedAt = DateTime.UtcNow;
        }
    }

}