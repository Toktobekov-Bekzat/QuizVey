using Microsoft.EntityFrameworkCore;
using QuizVey.Application.Interfaces;
using QuizVey.Domain.Entities;
using QuizVey.Infrastructure.Persistence;

namespace QuizVey.Infrastructure.Repositories;

public class AttemptRepository : IAttemptRepository
{
    private readonly ApplicationDbContext _context;

    public AttemptRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Attempt?> GetByIdAsync(Guid id)
    {
        return await _context.Attempts
            .Include(a => a.Answers)
            .Include(a => a.DraftAnswers)
            .FirstOrDefaultAsync(a => a.Id == id)
;   }

    public async Task SaveAsync(Attempt attempt)
    {
        await _context.SaveChangesAsync();
    }
}