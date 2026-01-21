using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Application.Interfaces;
using QuizVey.Application.UseCases.StartAttempt;

namespace QuizVey.Application.UseCases.StartAttempt
{
    public class StartAttemptHandler
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssessmentVersionRepository _assessmentVersionRepository;

        public StartAttemptHandler(IAssignmentRepository assignmentRepository, IAssessmentVersionRepository assessmentVersionRepository)
        {
            _assignmentRepository = assignmentRepository;
            _assessmentVersionRepository = assessmentVersionRepository;
        }

        public async Task<StartAttemptResult> Handle(StartAttemptCommand command)
        {
            var assignment = await _assignmentRepository
                .GetByIdAsync(command.AssignmentId);

            if (assignment == null) 
                throw new InvalidOperationException("Assignment not found");

            if (assignment.UserId != command.UserId)
                throw new InvalidOperationException("User is not authorized for this assignment");

            var version = await _assessmentVersionRepository
                .GetByIdAsync(assignment.AssessmentVersionId);

            if (version == null)
                throw new InvalidOperationException("Assessment version not found.");

            var attempt = assignment.StartAttempt(version);

            await _assignmentRepository.SaveAsync(assignment);

            return new StartAttemptResult(
                attempt.Id, 
                attempt.Status
            );
        }
        
    }
}