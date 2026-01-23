using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Application.Interfaces;

namespace QuizVey.Application.UseCases.UpdateQuestion
{
    public class UpdateQuestionHandler
    {
        private readonly IAssessmentVersionRepository _assessmentVersionRepository;

        public UpdateQuestionHandler(IAssessmentVersionRepository assessmentVersionRepository)
        {
            _assessmentVersionRepository = assessmentVersionRepository;
        }

        public async Task<UpdateQuestionResult> Handle(UpdateQuestionCommand command)
        {
            var version = await _assessmentVersionRepository.GetByIdAsync(command.AssessmentVersionId);

        if (version == null)
            throw new InvalidOperationException("Assessment version not found");

        // domain handles update logic + validation
        version.UpdateQuestion(
            command.QuestionId,
            command.Text,
            command.Description,
            command.Type,
            command.Options,
            command.CorrectAnswers
        );

        await _assessmentVersionRepository.SaveAsync(version);
            
            return new UpdateQuestionResult(command.QuestionId);
        }
    }
}