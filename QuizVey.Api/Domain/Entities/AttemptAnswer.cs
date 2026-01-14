using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using QuizVey.Api.Domain.Enums;

namespace QuizVey.Api.Domain.Entities
{
    public class AttemptAnswer : BaseEntity
    {
            public Guid AttemptId { get; private set; }
            public Guid QuestionId { get; private set; }
            public string Value { get; private set; }

            protected AttemptAnswer(){}

            internal AttemptAnswer(
                Guid attemptId,
                Guid questionId,
                string value)
            {
                AttemptId = attemptId;
                QuestionId = questionId;
                Value = value;
            }
    }
}