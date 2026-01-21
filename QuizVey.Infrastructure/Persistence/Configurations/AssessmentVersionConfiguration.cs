using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizVey.Domain.Entities;

namespace QuizVey.Infrastructure.Persistence.Configurations;

public class AssessmentVersionConfiguration : IEntityTypeConfiguration<AssessmentVersion>
{
    public void Configure(EntityTypeBuilder<AssessmentVersion> builder)
    {
        builder.ToTable("AssessmentVersions");

        builder.HasKey(av => av.Id);

        builder.Property(av => av.VersionNumber).IsRequired();
        builder.Property(av => av.Type).IsRequired();
        builder.Property(av => av.Status).IsRequired();

        builder.Property(av => av.PassingPercentage)
            .HasDefaultValue(50);

        // -------------------------------
        // Relationship: Assessment → Versions
        // -------------------------------
        builder.HasOne<Assessment>()
            .WithMany(a => a.Versions)
            .HasForeignKey(av => av.AssessmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // -------------------------------
        // Questions collection (backing field, no FK property)
        // -------------------------------
        builder
            .HasMany(av => av.Questions)
            .WithOne()
            .HasForeignKey("AssessmentVersionId")            // <── shadow FK
            .OnDelete(DeleteBehavior.Cascade);

        // Use backing-field mode
        builder.Metadata
            .FindNavigation(nameof(AssessmentVersion.Questions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
