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
            builder.Entity<Exam>()
                .HasOne(e => e.User)
                .WithMany(u => u.Exams)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<TakeAnswer>()
                .HasOne(t => t.TakeExam)
                .WithMany(t => t.TakeAnswers)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TakeAnswer>()
                .HasOne(a => a.AnswerOption)
                .WithMany(a => a.TakeAnswers)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TakeAnswer>()
                .HasOne(a => a.Question)
                .WithMany(a => a.TakeAnswers)
                .OnDelete(DeleteBehavior.Restrict);

            string SUPERADMIN_ID = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
            string SUPERADMIN_ROLE_ID = "fe59cdfb-998b-4b58-85b4-5a8600389721";
            string ADMIN_ROLE_ID = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";
            string STUDENT_ROLE_ID = "daab2d3d-6652-4ef9-9d43-0ec02c7dc78f";

            //Seed user roles
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                Id = SUPERADMIN_ROLE_ID,
                ConcurrencyStamp = SUPERADMIN_ROLE_ID
            },
            new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                Id = ADMIN_ROLE_ID,
                ConcurrencyStamp = ADMIN_ROLE_ID
            },
            new IdentityRole
            {
                Name = "Student",
                NormalizedName = "STUDENT",
                Id = STUDENT_ROLE_ID,
                ConcurrencyStamp = STUDENT_ROLE_ID
            });

            //Create Admin user
            var adminUser = new ApplicationUser
            {
                Id = SUPERADMIN_ID,
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "SuperAdmin",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM"
            };

            //Set Admin user password
            PasswordHasher<ApplicationUser> phAdmin = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = phAdmin.HashPassword(adminUser, "Admin_1234");

            //Seed user
            builder.Entity<ApplicationUser>().HasData(adminUser);

            //Set Admin role to Admin user
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = SUPERADMIN_ROLE_ID,
                UserId = SUPERADMIN_ID
            });

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