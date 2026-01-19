using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizVey.Domain.Entities;

namespace QuizVey.Infrastructure.Persistence.Configurations;

public class AttemptDraftAnswerConfiguration : IEntityTypeConfiguration<AttemptDraftAnswer>
{
    public void Configure(EntityTypeBuilder<AttemptDraftAnswer> builder)
    {
        builder.ToTable("AttemptDraftAnswers");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AttemptId).IsRequired();
        builder.Property(a => a.QuestionId).IsRequired();
        builder.Property(a => a.Value)
            .IsRequired()
            .HasMaxLength(2000);
    }
}
