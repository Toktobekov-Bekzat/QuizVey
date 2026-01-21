using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Application.Interfaces;

namespace QuizVey.Application.UseCases.RemoveQuestion
{
    public class RemoveQuestionHandler
    {
        private readonly IAssessmentVersionRepository _assessmentVersionRepository;

        public RemoveQuestionHandler(IAssessmentVersionRepository assessmentVersionRepository)
        {
            _assessmentVersionRepository = assessmentVersionRepository;
        }

        public async Task<RemoveQuestionResult> Handle(RemoveQuestionCommand command)
        {
            var version = await _assessmentVersionRepository.GetByIdAsync(command.AssessmentVersionId);

            if (version == null) throw new InvalidOperationException("Assessment version not found.");

            version.RemoveQuestion(command.QuestionId);

            await _assessmentVersionRepository.SaveAsync(version);
            
            return new RemoveQuestionResult(true);
        }
    }
}