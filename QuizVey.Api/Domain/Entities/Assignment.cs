using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizVey.Api.Domain.Entities
{
    public class Assignment : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid AssessmentVersionId { get; private set;}

        private readonly List<Attempt> _attempts = new ();

        public IReadOnlyCollection<Attempt> Attempts => _attempts.AsReadOnly();

        protected Assignment(){}

        public Assignment(Guid userId, Guid assessmentVersionId)
        {
            UserId = userId;
            AssessmentVersionId = assessmentVersionId;
        }

        public Attempt StartAttempt(AssessmentVersion version)
        {
            if (_attempts.Any(a => a.IsInProgress))
        throw new InvalidOperationException("An attempt is already in progress.");

        if (version.Type == AssessmentType.Survey && _attempts.Any())
            throw new InvalidOperationException("Surveys do not allow retakes.");

        if (version.Type == AssessmentType.Quiz)
        {
            if (!version.RetakesAllowed && _attempts.Any())
                throw new InvalidOperationException("Retakes are not allowed for this quiz.");

            if (version.MaxAttempts.HasValue &&
                _attempts.Count >= version.MaxAttempts.Value)
                throw new InvalidOperationException("Maximum number of attempts reached.");
        }

            var attempt = new Attempt();
            _attempts.Add(attempt);

            return attempt;
        }
            
    }
}