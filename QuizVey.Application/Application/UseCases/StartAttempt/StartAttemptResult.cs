using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Api.Domain.Enums;

namespace QuizVey.Api.Application.UseCases.StartAttempt;

public record StartAttemptResult(
    Guid AttemptId,
    AttemptStatus Status
);
