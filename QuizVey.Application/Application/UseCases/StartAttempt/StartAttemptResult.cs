using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Domain.Enums;

namespace QuizVey.Application.UseCases.StartAttempt;

public record StartAttemptResult(
    Guid AttemptId,
    AttemptStatus Status
);
