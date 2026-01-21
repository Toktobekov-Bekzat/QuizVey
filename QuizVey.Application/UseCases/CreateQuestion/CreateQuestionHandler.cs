using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Application.Interfaces;
using QuizVey.Domain.Entities;

namespace QuizVey.Application.UseCases.CreateQuestion
{
    public class CreateQuestionHandler
    {
        private readonly IAssessmentVersionRepository _assessmentVersionRepository;

        public CreateQuestionHandler(IAssessmentVersionRepository assessmentVersionRepository)
        {
            _assessmentVersionRepository = assessmentVersionRepository;
        }

    public async Task<CreateQuestionResult> Handle(CreateQuestionCommand command)
        {
            var assessmentVersion = await _assessmentVersionRepository.GetByIdAsync(command.AssessmentVersionId);

            if (assessmentVersion == null)
            {
                throw new InvalidOperationException("Assessment version not found");
            }

            var question = new Question(command.Text, command.Type, command.Description);

            if (command.Options != null && command.CorrectAnswers != null)
            {
                question.SetOptions(command.Options, command.CorrectAnswers);
            }

            assessmentVersion.AddQuestion(question);

            await _assessmentVersionRepository.SaveAsync(assessmentVersion);
            return new CreateQuestionResult(question.Id);
            
        }
        
    }
}