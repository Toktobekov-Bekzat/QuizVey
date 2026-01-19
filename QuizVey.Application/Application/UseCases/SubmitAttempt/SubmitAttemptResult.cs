using QuizVey.Api.Domain.Enums;

namespace QuizVey.Api.Application.UseCases.SubmitAttempt;

public record SubmitAttemptResult(
    AttemptStatus Status
);
