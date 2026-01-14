namespace QuizVey.Api.Application.UseCases.SaveDraftAnswer;

public record SaveDraftAnswerCommand(
    Guid AttemptId,
    Guid UserId,
    Guid QuestionId,
    string Value
);