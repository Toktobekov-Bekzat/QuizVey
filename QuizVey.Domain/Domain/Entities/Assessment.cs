using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Domain.Enums;

namespace QuizVey.Domain.Entities
{
    public class Assessment : BaseEntity
    {
        public string Title { get; private set;}
        public string? Description { get; private set; }

        public AssessmentStatus Status {get; private set;} = AssessmentStatus.Draft;

        private readonly List<AssessmentVersion> _versions = new();
        public IReadOnlyCollection<AssessmentVersion> Versions => _versions.AsReadOnly();

        protected Assessment(){}

        public Assessment(string title, string? description = null)
        {
            Title = title;
            Description = description;
        }

        public AssessmentVersion CreateNewVersionFromActive()
        {
            var activeVersion = _versions.SingleOrDefault(v => v.Status == AssessmentVersionStatus.Active);

            if (activeVersion == null) throw new InvalidOperationException("No active version exists to duplicate");

            var newVersion = activeVersion.CloneAsDraft();

            _versions.Add(newVersion);

            return newVersion;
        }

        public AssessmentVersion CreateVersion(
            AssessmentType type,
            bool retakesAllowed = false,
            int? maxAttempts = null
        )
        {
            int nextVersionNumber = _versions.Count == 0
                ? 1
                : _versions.Max(v => v.VersionNumber) + 1;

            var version = new AssessmentVersion(
                this.Id,
                nextVersionNumber,
                type,
                retakesAllowed,
                maxAttempts
            );

            _versions.Add(version);

            return version;
        }


        

    }
}