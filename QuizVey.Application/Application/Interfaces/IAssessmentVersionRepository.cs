using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Api.Domain.Entities;

namespace QuizVey.Api.Application.Interfaces
{
    public interface IAssessmentVersionRepository
    {
        Task<AssessmentVersion?> GetByIdAsync(Guid id);
    }
}