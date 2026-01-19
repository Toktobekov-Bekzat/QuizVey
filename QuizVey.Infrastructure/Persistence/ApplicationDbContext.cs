using Microsoft.EntityFrameworkCore;
using QuizVey.Domain.Entities;

namespace QuizVey.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ────────────────────────────────
        // DbSets (tables)
        // ────────────────────────────────
        public DbSet<Assessment> Assessments => Set<Assessment>();
        public DbSet<AssessmentVersion> AssessmentVersions => Set<AssessmentVersion>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Assignment> Assignments => Set<Assignment>();
        public DbSet<Attempt> Attempts => Set<Attempt>();
        public DbSet<AttemptAnswer> AttemptAnswers => Set<AttemptAnswer>();
        public DbSet<AttemptDraftAnswer> AttemptDraftAnswers => Set<AttemptDraftAnswer>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Load all IEntityTypeConfiguration<T> from the same assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // OPTIONAL: globally disable cascade deletes (safer)
            foreach (var fk in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}