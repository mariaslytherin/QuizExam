using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.TakeQuestion;
using QuizExam.Core.Services;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Identity;
using QuizExam.Infrastructure.Data.Repositories;
using QuizExam.Test.Constants;

namespace QuizExam.Test.TakeAnswerServiceTests
{
    public class TakeAnswerServiceTests
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
                .AddSingleton<ITakeAnswerService, TakeAnswerService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void AddAnswerShouldThrowExceptionIfQuestionDoesNotExist()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = Guid.NewGuid().ToString(),
                TakeExamId = UniqueIdentifiersTestConstants.TakeId,
                CheckedOptionId = UniqueIdentifiersTestConstants.AOptionId,
                ExamId = UniqueIdentifiersTestConstants.ExamId_Bg,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();

            Assert.CatchAsync<DbUpdateException>(async () => await service.AddAnswer(model, UniqueIdentifiersTestConstants.ExamId_Bg));
        }

        [Test]
        public async Task AddAnswerShouldReturnFalseIfTakeExamDoesNotExist()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = UniqueIdentifiersTestConstants.QuestionId,
                TakeExamId = Guid.NewGuid().ToString(),
                CheckedOptionId = UniqueIdentifiersTestConstants.AOptionId,
                ExamId = UniqueIdentifiersTestConstants.ExamId_Bg,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();
            var result = await service.AddAnswer(model, UniqueIdentifiersTestConstants.ExamId_Bg);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AddAnswerShouldReturnTrueIfCheckedOptionIdEqualsAnsweOptionId()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = UniqueIdentifiersTestConstants.QuestionId,
                TakeExamId = UniqueIdentifiersTestConstants.TakeId,
                CheckedOptionId = UniqueIdentifiersTestConstants.AOptionId,
                ExamId = UniqueIdentifiersTestConstants.ExamId_Bg,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();
            var result = await service.AddAnswer(model, UniqueIdentifiersTestConstants.ExamId_Bg);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task AddAnswerShouldReturnTrueIfCheckedOptionIdDoesNotEqualAnswerOptionId()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = UniqueIdentifiersTestConstants.QuestionId,
                TakeExamId = UniqueIdentifiersTestConstants.TakeId,
                CheckedOptionId = UniqueIdentifiersTestConstants.BOptionId,
                ExamId = UniqueIdentifiersTestConstants.ExamId_Bg,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();
            var result = await service.AddAnswer(model, UniqueIdentifiersTestConstants.ExamId_Bg);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task AddAnswerShouldReturnTrueIfTakeExamAndQuestionExist()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = UniqueIdentifiersTestConstants.QuestionId,
                TakeExamId = UniqueIdentifiersTestConstants.TakeId,
                CheckedOptionId = UniqueIdentifiersTestConstants.BOptionId,
                ExamId = UniqueIdentifiersTestConstants.ExamId_Bg,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();
            var result = await service.AddAnswer(model, UniqueIdentifiersTestConstants.ExamId_Bg);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task AddAnswerShouldReturnFalseIfTakeAnswerAndTakeExamDoNotExist()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = UniqueIdentifiersTestConstants.QuestionId,
                TakeExamId = Guid.NewGuid().ToString(),
                ExamId = UniqueIdentifiersTestConstants.ExamId_Bg,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();
            var result = await service.AddAnswer(model, UniqueIdentifiersTestConstants.ExamId_Bg);

            Assert.That(result, Is.False);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
            serviceProvider.Dispose();
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
                    },
                    new AnswerOption()
                    {
                        Id = UniqueIdentifiersTestConstants.BOptionId.ToGuid(),
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
