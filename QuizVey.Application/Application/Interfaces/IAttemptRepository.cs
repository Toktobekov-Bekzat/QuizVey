using QuizVey.Domain.Entities;

namespace QuizVey.Application.Interfaces
{
    public interface IAttemptRepository
    {
        Task<Attempt?> GetByIdAsync(Guid id);
        Task SaveAsync(Attempt attempt);
    }
}