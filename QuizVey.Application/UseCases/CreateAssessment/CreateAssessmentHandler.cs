using QuizVey.Application.Interfaces;
using QuizVey.Application.UseCases.CreateAssessment;
using QuizVey.Application.UseCases.CreateAssignment;
using QuizVey.Domain.Entities;
using QuizVey.Domain.Enums;

namespace QuizVey.Application.UseCases.CreateAssessment;

public class CreateAssessmentHandler
{
    private readonly IAssessmentRepository _assessmentRepository;

    public CreateAssessmentHandler(IAssessmentRepository assessmentRepository)
    {
        _assessmentRepository = assessmentRepository;
    }

    public async Task<CreateAssessmentResult> Handle(CreateAssessmentCommand command)
    {
        var assessment = new Assessment(command.Title, command.Description);
        await _assessmentRepository.AddAsync(assessment);

        return new CreateAssessmentResult(assessment.Id);
    }
    
}