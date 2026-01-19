using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizVey.Domain.Entities;

namespace QuizVey.Infrastructure.Persistence.Configurations;

public class AttemptAnswerConfiguration : IEntityTypeConfiguration<AttemptAnswer>
{
    public void Configure(EntityTypeBuilder<AttemptAnswer> builder)
    {
        builder.ToTable("AttemptAnswers");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AttemptId).IsRequired();
        builder.Property(a => a.QuestionId).IsRequired();
        builder.Property(a => a.Value)
            .IsRequired()
            .HasMaxLength(2000);
    }
}
