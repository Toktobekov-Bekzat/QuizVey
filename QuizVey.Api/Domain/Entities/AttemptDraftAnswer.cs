using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using QuizVey.Api.Domain.Enums;

namespace QuizVey.Api.Domain.Entities
{
    public class AttemptDraftAnswer : BaseEntity
    {
        public Guid AttemptId { get; private set; }
        public Guid QuestionId { get; private set; }
        public string Value { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected AttemptDraftAnswer() { }

        internal AttemptDraftAnswer(
            Guid attemptId,
            Guid questionId,
            string value)
        {
            AttemptId = attemptId;
            QuestionId = questionId;
            Value = value;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void Update(string value)
        {
            Value = value;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
