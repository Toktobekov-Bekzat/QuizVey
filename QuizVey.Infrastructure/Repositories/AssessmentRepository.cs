using Microsoft.EntityFrameworkCore;
using QuizVey.Application.Interfaces;
using QuizVey.Domain.Entities;
using QuizVey.Infrastructure.Persistence;

namespace QuizVey.Infrastructure.Repositories;

public class AssessmentRepository : IAssessmentRepository
{
    private readonly ApplicationDbContext _context;

    public AssessmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Assessment assessment)
    {
        await _context.Assessments.AddAsync(assessment);
        await _context.SaveChangesAsync();
    }

    public async Task<Assessment?> GetByIdAsync(Guid id)
    {
        return await _context.Assessments
            .Include(a => a.Versions)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveAsync(Assessment assessment)
    {
        _context.Assessments.Update(assessment);
        await _context.SaveChangesAsync();

    }
}