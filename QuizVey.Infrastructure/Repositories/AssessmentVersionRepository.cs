using Microsoft.EntityFrameworkCore;
using QuizVey.Application.Interfaces;
using QuizVey.Domain.Entities;
using QuizVey.Infrastructure.Persistence;

namespace QuizVey.Infrastructure.Repositories;

public class AssessmentVersionRepository : IAssessmentVersionRepository
{
    private readonly ApplicationDbContext _context;

    public AssessmentVersionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AssessmentVersion?> GetByIdAsync(Guid id)
    {
        return await _context.AssessmentVersions
            .Include(v => v.Questions.OrderBy(q => q.Order))
            .FirstOrDefaultAsync(v => v.Id == id);
    }
}