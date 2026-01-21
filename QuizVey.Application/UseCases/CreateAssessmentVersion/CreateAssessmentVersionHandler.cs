using QuizVey.Application.Interfaces;
using QuizVey.Application.UseCases.CreateAssignment;
using QuizVey.Domain.Entities;

namespace QuizVey.Application.UseCases.CreateAssessmentVersion;

public class CreateAssessmentVersionHandler
{
    private readonly IAssessmentRepository _assessmentRepository;

    public CreateAssessmentVersionHandler(IAssessmentRepository assessmentRepository)
    {
        _assessmentRepository = assessmentRepository;
    }

    public async Task<CreateAssessmentVersionResult> Hanle(CreateAssessmentVersionCommand command)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.AssessmentId);
        if (assessment == null)
            throw new InvalidOperationException("Assessment not found.");

        var version = assessment.CreateVersion(
        command.Type,
        command.RetakesAllowed,
        command.MaxAttemptsAllowed);

        await _assessmentRepository.SaveAsync(assessment);

        return new CreateAssessmentVersionResult(version.Id);
    }
}