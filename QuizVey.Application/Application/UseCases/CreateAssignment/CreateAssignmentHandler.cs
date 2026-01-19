using QuizVey.Api.Application.Interfaces;
using QuizVey.Api.Domain.Entities;
using QuizVey.Api.Domain.Enums;

namespace QuizVey.Api.Application.UseCases.CreateAssignment;

public class CreateAssignmentHandler
{
    private readonly IAssessmentVersionRepository _assessmentVersionRepository;
    private readonly IAssignmentRepository _assignmentRepository;

    public CreateAssignmentHandler(
        IAssessmentVersionRepository assessmentVersionRepository, 
        IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
        _assessmentVersionRepository = assessmentVersionRepository;
    }
        public async Task<CreateAssignmentResult> Handle(CreateAssignmentCommand command)
    {
        var assessmentVersion = await _assessmentVersionRepository.GetByIdAsync(command.AssessmentVersionId);

        if (assessmentVersion == null)
        {
            throw new InvalidOperationException("Assessment version not found.");
        }
        if (assessmentVersion.Status == AssessmentVersionStatus.Archived)
        {
            throw new InvalidOperationException("Archived assessment versions cannot be assigned");
        }
        if (await _assignmentRepository.ExistsAsync(command.UserId, command.AssessmentVersionId)){
            throw new InvalidOperationException("User is already assigned to this assessment.");
        }

        var assignment = new Assignment(command.UserId, command.AssessmentVersionId);

        await _assignmentRepository.AddAsync(assignment);

        return new CreateAssignmentResult(assignment.Id);

    }
}