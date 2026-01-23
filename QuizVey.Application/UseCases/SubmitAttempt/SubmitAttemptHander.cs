using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using QuizVey.Application.Interfaces;
using QuizVey.Application.UseCases.SubmitAttempt;
using QuizVey.Domain.Enums;

namespace QuizVey.Application.UseCases.SubmitAttempt
{
    public class SubmitAttemptHandler
    {
        private readonly IAttemptRepository _attemptRepository;
        private readonly IAssessmentVersionRepository _assessmentVersionRepository;
        private readonly IAssessmentRepository _assessmentRepository;

        public SubmitAttemptHandler(
            IAttemptRepository attemptRepository,
            IAssessmentVersionRepository assessmentVersionRepository,
            IAssessmentRepository assessmentRepository)
        {
            _attemptRepository = attemptRepository;
            _assessmentVersionRepository = assessmentVersionRepository;
            _assessmentRepository = assessmentRepository;
        }

        public async Task<SubmitAttemptResult> Handle(SubmitAttemptCommand command)
        {
            // 1. Load attempt
            var attempt = await _attemptRepository.GetByIdAsync(command.AttemptId)
                ?? throw new InvalidOperationException("Attempt not found.");

            if (command.UserId != attempt.UserId)
                throw new UnauthorizedAccessException();

            // 2. Load version
            var version = await _assessmentVersionRepository
                .GetByIdAsync(attempt.AssessmentVersionId)
                ?? throw new InvalidOperationException("Assessment version not found.");

            // 3. Load parent assessment to read its TYPE
            var assessment = await _assessmentRepository.GetByIdAsync(version.AssessmentId)
                ?? throw new InvalidOperationException("Parent assessment not found.");

            // 4. Process answers
            var finalAnswers = attempt.SubmitDraftAnswers();

            if (assessment.Type == AssessmentType.Quiz)
            {
                var passed = version.EvaluateQuiz(finalAnswers);
                attempt.CompleteQuiz(passed);
            }
            else
            {
                attempt.CompleteSurvey();
            }

            // 5. Save
            await _attemptRepository.SaveAsync(attempt);

            return new SubmitAttemptResult(attempt.Status);
        }
        
    }
}
