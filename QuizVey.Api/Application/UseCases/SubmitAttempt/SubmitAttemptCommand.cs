namespace QuizVey.Api.Application.UseCases.SubmitAttempt;

public record SubmitAttemptCommand(
    Guid AttemptId,
    Guid UserId
);

