using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizExam.Infrastructure.Data.Identity;
using QuizExam.Infrastructure.InitialSeed;

namespace QuizExam.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TakeAnswer>()
                .HasOne(t => t.TakeExam)
                .WithMany(t => t.TakeAnswers)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TakeAnswer>()
                .HasOne(a => a.AnswerOption)
                .WithMany(a => a.TakeAnswers)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ApplyConfiguration(new InitialDataConfiguration<Subject>(@"InitialSeed/subjects.json"));

            base.OnModelCreating(builder);
        }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<AnswerOption> AnswerOptions { get; set; }

        public DbSet<TakeExam> TakeExams { get; set; }

        public DbSet<TakeAnswer> TakeAnswers { get; set; }
    }
}