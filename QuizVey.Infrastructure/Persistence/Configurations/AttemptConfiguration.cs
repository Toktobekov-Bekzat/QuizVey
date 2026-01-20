using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizVey.Domain.Entities;

namespace QuizVey.Infrastructure.Persistence.Configurations;

public class AttemptConfiguration : IEntityTypeConfiguration<Attempt>
{
    public void Configure(EntityTypeBuilder<Attempt> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.AssessmentVersionId).IsRequired();
        builder.Property(x => x.Status).IsRequired();

        // Ignore public read-only navigations
        builder.Ignore(x => x.Answers);
        builder.Ignore(x => x.DraftAnswers);

        // FINAL answers (backing field)
        builder
            .HasMany<AttemptAnswer>("_finalAnswers")
            .WithOne()
            .HasForeignKey(x => x.AttemptId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation("_finalAnswers")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // DRAFT answers (backing field)
        builder
            .HasMany<AttemptDraftAnswer>("_draftAnswers")
            .WithOne()
            .HasForeignKey(x => x.AttemptId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation("_draftAnswers")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

