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

    public async Task<CreateAssessmentVersionResult> Handle(CreateAssessmentVersionCommand command)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.AssessmentId);
        if (assessment == null)
            throw new InvalidOperationException("Assessment not found.");

        // Create a cloned version from the latest version
        var version = assessment.CreateNewVersion();

        await _assessmentRepository.SaveAsync(assessment);

        return new CreateAssessmentVersionResult(version.Id);
    }
}
