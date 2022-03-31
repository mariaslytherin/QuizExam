using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizExam.Infrastructure.Data.Identity;

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
            builder.Entity<Question>()
                .HasIndex(c => c.OrderNumber)
                .IsUnique();

            base.OnModelCreating(builder);
        }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<AnswerOption> AnswerOptions { get; set; }
    }
}