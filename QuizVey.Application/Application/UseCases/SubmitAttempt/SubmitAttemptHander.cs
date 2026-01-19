using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using QuizVey.Api.Application.Interfaces;
using QuizVey.Api.Domain.Enums;

namespace QuizVey.Api.Application.UseCases.SubmitAttempt
{
    public class SubmitAttemptHandler
    {
        private readonly IAttemptRepository _attemptRepository;
        private readonly IAssessmentVersionRepository _assessmentVersionRepository;

        public SubmitAttemptHandler(
            IAttemptRepository attemptRepository, 
            IAssessmentVersionRepository assessmentVersionRepository)
        {
            _attemptRepository = attemptRepository;
            _assessmentVersionRepository = assessmentVersionRepository;
        }

        public async Task<SubmitAttemptResult> Handle(SubmitAttemptCommand command)
        {
            var attempt = await _attemptRepository.GetByIdAsync(command.AttemptId);

            if (attempt == null)
            {
                throw new InvalidOperationException("Attempt not found.");
            }

            if (command.UserId != attempt.UserId)
            {
                throw new UnauthorizedAccessException();
            }

            var version = await _assessmentVersionRepository
                .GetByIdAsync(attempt.AssessmentVersionId)
                ?? throw new InvalidOperationException("Assessment version not found.");

            var finalAnswers = attempt.SubmitDraftAnswers();

            if (version.Type == AssessmentType.Quiz)
            {
                var passed = version.Evaluate(finalAnswers);
                attempt.CompleteQuiz(passed);
            }
            else
            {
                attempt.CompleteSurvey();
            }

            await _attemptRepository.SaveAsync(attempt);

            return new SubmitAttemptResult(attempt.Status);
        }
        
    }
}
