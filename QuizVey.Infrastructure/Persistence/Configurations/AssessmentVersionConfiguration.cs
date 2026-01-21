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

        builder.HasOne<Assessment>()
            .WithMany("Versions")
            .HasForeignKey(av => av.AssessmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Metadata
            .FindNavigation(nameof(AssessmentVersion.Questions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(v => v.Questions)
            .WithOne()
            .HasForeignKey(q => q.AssessmentVersionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Metadata
            .FindNavigation(nameof(AssessmentVersion.Questions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

    }
}