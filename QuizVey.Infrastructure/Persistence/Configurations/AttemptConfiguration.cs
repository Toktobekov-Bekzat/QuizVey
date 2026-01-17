using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizVey.Api.Domain.Entities;

namespace QuizVey.Infrastructure.Persistence.Configurations;

public class AttemptConfiguration : IEntityTypeConfiguration<Attempt>
{
    public void Configure(EntityTypeBuilder<Attempt> builder)
    {
        builder.ToTable("Attempts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserId).IsRequired();
        builder.Property(a => a.AssessmentVersionId).IsRequired();
        builder.Property(a => a.Status).IsRequired();

        builder.Metadata
            .FindNavigation(nameof(Attempt.Answers))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata
            .FindNavigation(nameof(Attempt.DraftAnswers))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
