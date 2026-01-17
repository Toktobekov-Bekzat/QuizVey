using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using QuizVey.Api.Domain.Entities;

namespace QuizVey.Api.Application.Interfaces
{
    public interface IAssignmentRepository
    {
        Task AddAsync(Assignment assignment);
        Task<Assignment?> GetByIdAsync(Guid assignmentId);
        Task SaveAsync(Assignment assignment);

        Task<bool> ExistsAsync(Guid userId, Guid AssessmentVersionId);
    }
}