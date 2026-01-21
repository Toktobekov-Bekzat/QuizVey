using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizVey.Application.UseCases.CreateAssignment;

public record CreateAssignmentCommand(
    Guid AssessmentVersionId,
    Guid UserId
);