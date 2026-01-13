using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizVey.Api.Application.UseCases
{
    public record StartAttemptResult
    {
        Guid AssignmentId;
        Guid UserId;
    }
}