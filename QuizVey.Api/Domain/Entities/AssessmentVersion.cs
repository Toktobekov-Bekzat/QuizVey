using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Api.Domain.Enums;

namespace QuizVey.Api.Domain.Entities
{
    public class AssessmentVersion : BaseEntity
    {
        public Guid AssessmentId { get; private set; }
        public int VersionNumber { get; private set; }
        public AssessmentType Type { get; set; }

        // Quiz-only configuration
        public bool RetakesAllowed { get; private set; }
        public int? MaxAttempts { get; private set; }
        public AssessmentVersionStatus Status { get; private set;} = AssessmentVersionStatus.Draft;

        private readonly List<Question> _questions = new();

        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

        protected AssessmentVersion(){}

        internal AssessmentVersion(
            Guid assessmentId, 
            int versionNumber,
            AssessmentType type,
            bool retakesAllowed = false,
            int? maxAttempts = null)
        {
            AssessmentId = assessmentId;
            VersionNumber = versionNumber;
            Type = type;
            RetakesAllowed = retakesAllowed;
            MaxAttempts = maxAttempts;
        }

        public AssessmentVersion CloneAsDraft()
        {
            if (Status != AssessmentVersionStatus.Active) throw new InvalidOperationException("Only active version can be cloned.");
            
            var clone = new AssessmentVersion(AssessmentId, VersionNumber + 1);

            foreach (var question in _questions)
            {
                clone._questions.Add(question.Clone());
            }

            return clone;
        }

        public void Activate()
        {
            if (Status != AssessmentVersionStatus.Draft) throw new InvalidOperationException("Only draft versions can be activated.");

            Status = AssessmentVersionStatus.Active;
        }

        public void Archive()
        {
            if (Status != AssessmentVersionStatus.Active) throw new InvalidOperationException("Only active versions can be archived.");

            Status = AssessmentVersionStatus.Archived;
        }

    }
}