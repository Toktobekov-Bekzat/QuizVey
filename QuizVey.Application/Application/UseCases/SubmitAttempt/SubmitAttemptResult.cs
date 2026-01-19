using QuizVey.Domain.Enums;

namespace QuizVey.Application.UseCases.SubmitAttempt;

public record SubmitAttemptResult(
    AttemptStatus Status
);
