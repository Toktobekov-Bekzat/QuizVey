using QuizVey.Api.Domain.Entities;

namespace QuizVey.Api.Application.Interfaces
{
    public interface IAttemptRepository
    {
        Task<Attempt?> GetByIdAsync(Guid id);
        Task SaveAsync(Attempt attempt);
    }
}