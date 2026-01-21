using QuizVey.Domain.Enums;

namespace QuizVey.Application.UseCases.CreateAssignment;

public record CreateAssessmentVersionCommand(
    Guid AssessmentId,
    int VersionNumber,
    AssessmentType Type,
    bool RetakesAllowed,
    int? MaxAttemptsAllowed
);