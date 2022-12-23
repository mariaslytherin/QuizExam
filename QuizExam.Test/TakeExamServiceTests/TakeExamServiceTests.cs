using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using QuizExam.Core.Contracts;
using QuizExam.Core.Services;
using QuizExam.Infrastructure.Data.Identity;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Repositories;
using QuizExam.Test.Constants;
using QuizExam.Core.Extensions;

namespace QuizExam.Test.TakeExamServiceTests
{
    public class TakeExamServiceTests
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<ITakeExamService, TakeExamService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {
            var subject = new Subject()
            {
                Id = UniqueIdentifiersTestConstants.SubjectId_Bg.ToGuid(),
                Name = "Български език и литература"
            };

            var exam = new Exam()
            {
                Id = UniqueIdentifiersTestConstants.ExamId_Bg.ToGuid(),
                Title = "Български език и литература (12 клас)",
                Description = "Тест по БЕЛ за ученици в 12 клас.",
                MaxScore = 2,
                SubjectId = UniqueIdentifiersTestConstants.SubjectId_Bg.ToGuid(),
                IsActive = true,
            };

            var question = new Question()
            {
                Id = UniqueIdentifiersTestConstants.QuestionId.ToGuid(),
                Content = "А, Б, В...?",
                Points = 2,
                ExamId = UniqueIdentifiersTestConstants.ExamId_Bg.ToGuid(),
                Answers = new List<AnswerOption>()
                {
                    new AnswerOption()
                    {
                        Id = UniqueIdentifiersTestConstants.AOptionId.ToGuid(),
                        QuestionId = UniqueIdentifiersTestConstants.QuestionId.ToGuid(),
                        Content = "Some Option"
                    }
                },
            };

            var role = new IdentityRole
            {
                Name = "Admin",
                Id = UniqueIdentifiersTestConstants.RoleId,
                ConcurrencyStamp = UniqueIdentifiersTestConstants.RoleId
            };

            var user = new ApplicationUser
            {
                Id = UniqueIdentifiersTestConstants.UserId,
                Email = "user@user.com",
                NormalizedEmail = "USER@USER.COM",
                EmailConfirmed = true,
                FirstName = "User",
                LastName = "User",
                UserName = "user@user.com",
            };

            var take = new TakeExam()
            {
                Id = UniqueIdentifiersTestConstants.TakeId.ToGuid(),
                UserId = UniqueIdentifiersTestConstants.UserId,
                ExamId = UniqueIdentifiersTestConstants.ExamId_Bg.ToGuid(),
                TakeAnswers = new List<TakeAnswer>()
                {
                    new TakeAnswer()
                    {
                        TakeExamId = UniqueIdentifiersTestConstants.TakeId.ToGuid(),
                        QuestionId = UniqueIdentifiersTestConstants.QuestionId.ToGuid(),
                        AnswerOptionId = UniqueIdentifiersTestConstants.AOptionId.ToGuid()
                    }
                }
            };

            await repo.AddAsync(subject);
            await repo.AddAsync(exam);
            await repo.AddAsync(question);
            await repo.AddAsync(role);
            await repo.AddAsync(user);
            await repo.AddAsync(take);
            await repo.SaveChangesAsync();
        }
    }
}
