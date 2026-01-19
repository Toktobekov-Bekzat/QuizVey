using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Domain.Entities;

namespace QuizVey.Application.Interfaces
{
    public interface IAssessmentVersionRepository
    {
        Task<AssessmentVersion?> GetByIdAsync(Guid id);
    }
}