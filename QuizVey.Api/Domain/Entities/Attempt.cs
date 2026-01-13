using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace QuizVey.Api.Domain.Entities
{
    public class Attempt : BaseEntity
    {
        public AttemptStatus Status { get; private set; } = AttemptStatus.NotStarted;
        public DateTime? StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        protected Attempt(){}

        internal Attempt(bool startImmediately = true)
        {
            if (startImmediately)
                Start();
        }

        public bool IsInProgress => Status == AttemptStatus.InProgress;
        public bool IsCompleted =>
            Status == AttemptStatus.Completed ||
            Status == AttemptStatus.Passed ||
            Status == AttemptStatus.Failed;

        public void Start()
        {
            if (Status != AttemptStatus.NotStarted)
                throw new InvalidOperationException("Attempt already started.");

            Status = AttemptStatus.InProgress;
            StartedAt = DateTime.UtcNow;
        }

        public void CompleteSurvey()
        {
            if (Status != AttemptStatus.InProgress)
                throw new InvalidOperationException("Survey attempt must be in progress.");

            Status = AttemptStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void CompleteQuiz(bool passed)
        {
            if (Status != AttemptStatus.InProgress)
                throw new InvalidOperationException("Quiz attempt must be in progress.");

            Status = passed ? AttemptStatus.Passed : AttemptStatus.Failed;
            CompletedAt = DateTime.UtcNow;
        }

    }
}