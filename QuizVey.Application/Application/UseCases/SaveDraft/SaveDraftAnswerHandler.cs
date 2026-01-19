using QuizVey.Application.Interfaces;

namespace QuizVey.Application.UseCases.SaveDraftAnswer;

public class SaveDraftAnswerHandler
{
    private readonly IAttemptRepository _attemptRepository;

    public SaveDraftAnswerHandler(IAttemptRepository attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task Handle(SaveDraftAnswerCommand command)
    {
        var attempt = await _attemptRepository.GetByIdAsync(command.AttemptId);

        if (attempt == null)
            throw new InvalidOperationException("Attempt not found.");
        
        if (attempt.UserId != command.UserId)
        {
            throw new UnauthorizedAccessException();
        }
        attempt.SaveDraftAnswer(command.QuestionId, command.Value);
        await _attemptRepository.SaveAsync(attempt);
    }
}