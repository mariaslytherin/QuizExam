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
            builder.ApplyConfiguration(new InitialDataConfiguration<Subject>(@"InitialSeed/subjects.json"));

            base.OnModelCreating(builder);
        }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<AnswerOption> AnswerOptions { get; set; }
    }
}