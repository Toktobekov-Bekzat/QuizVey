using System;
using System.Collections.Generic;
using System.Linq;
using QuizVey.Domain.Enums;

namespace QuizVey.Domain.Entities
{
    public class Assessment : BaseEntity
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }

        /// <summary>
        /// The type of this assessment (Quiz or Survey). All versions must match this.
        /// </summary>
        public AssessmentType Type { get; private set; }

        public AssessmentStatus Status { get; private set; } = AssessmentStatus.Draft;

        private readonly List<AssessmentVersion> _versions = new();
        public IReadOnlyCollection<AssessmentVersion> Versions => _versions.AsReadOnly();

        protected Assessment() {}

        // ---------------------------------------------------
        //  Constructor — ALWAYS creates Version 1
        // ---------------------------------------------------
        public Assessment(
            string title,
            AssessmentType type,
            string? description = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Assessment title cannot be empty.");

            Title = title;
            Description = description;
            Type = type;

            // First version automatically created
            var firstVersion = new AssessmentVersion(
                assessmentId: this.Id,
                versionNumber: 1,
                retakesAllowed: false,
                maxAttempts: null
            );

            _versions.Add(firstVersion);
        }

        // ---------------------------------------------------
        //  Create next version by cloning the latest version
        // ---------------------------------------------------
        public AssessmentVersion CreateNewVersion()
        {
            if (_versions.Count == 0)
                throw new InvalidOperationException("Cannot create new version without an existing one.");

            var lastVersion = _versions
                .OrderByDescending(v => v.VersionNumber)
                .First();

            int nextVersionNumber = lastVersion.VersionNumber + 1;

            var clone = new AssessmentVersion(
                assessmentId: this.Id,
                versionNumber: nextVersionNumber,
                retakesAllowed: lastVersion.RetakesAllowed,
                maxAttempts: lastVersion.MaxAttempts
            );

            // Clone questions in correct order
            foreach (var question in lastVersion.Questions.OrderBy(q => q.Order))
            {
                var cloned = question.Clone();
                cloned.SetOrder(question.Order);
                clone.AddClonedQuestion(cloned);
            }

            _versions.Add(clone);
            return clone;
        }

        // ---------------------------------------------------
        //  Update base assessment info
        // ---------------------------------------------------
        public void Update(string title, string? description)
        {
            Title = title;
            Description = description;
        }

        // ---------------------------------------------------
        //  Status changes
        // ---------------------------------------------------
        public void Activate()
        {
            if (Status != AssessmentStatus.Draft)
                throw new InvalidOperationException("Only draft assessments can be activated.");

            Status = AssessmentStatus.Active;
        }

        public void Close()
        {
            if (Status != AssessmentStatus.Active)
                throw new InvalidOperationException("Only active assessments can be closed.");

            Status = AssessmentStatus.Closed;
        }
    }
}
