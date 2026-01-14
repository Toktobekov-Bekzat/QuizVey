using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using QuizVey.Api.Domain.Enums;

namespace QuizVey.Api.Domain.Entities
{
    public class Assessment : BaseEntity
    {
        public string Title { get; private set;}
        public string Description { get; private set; } 

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

        

    }
}