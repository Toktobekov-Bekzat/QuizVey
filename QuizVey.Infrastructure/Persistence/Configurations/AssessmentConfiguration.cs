using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizVey.Domain.Entities;

namespace QuizVey.Infrastructure.Persistence.Configurations;
public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> builder)
    {
        builder.ToTable("Assessments");

        builder.HasKey(x => x.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.Description)
            .HasMaxLength(2000);

        // Tell EF to use the private field for versions
        builder.Metadata
            .FindNavigation(nameof(Assessment.Versions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}