using Microsoft.EntityFrameworkCore;
using QuizVey.Application.Interfaces;
using QuizVey.Domain.Entities;
using QuizVey.Infrastructure.Persistence;

namespace QuizVey.Infrastructure.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ApplicationDbContext _context;

    public AssignmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Assignment assignment)
    {
        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid assessmentVersionId)
    {
        return await _context.Assignments
            .AnyAsync(a => 
                a.UserId == userId &&
                a.AssessmentVersionId == assessmentVersionId
            );
    }

    public async Task<Assignment?> GetByIdAsync(Guid assignmentId)
    {
        return await _context.Assignments
            .Include(a => a.Attempts)
            .ThenInclude(attempt => attempt.Answers)
            .Include(a => a.Attempts)
            .ThenInclude(attempt => attempt.DraftAnswers)
            .FirstOrDefaultAsync(a => a.Id == assignmentId);
    }

    public async Task SaveAsync(Assignment assignment)
    {
        await _context.SaveChangesAsync();
    }
}