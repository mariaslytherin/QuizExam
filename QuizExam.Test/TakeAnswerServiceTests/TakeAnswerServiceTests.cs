using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.TakeQuestion;
using QuizExam.Core.Models.User;
using QuizExam.Core.Services;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Identity;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Test.TakeAnswerServiceTests
{
    public class TakeAnswerServiceTests
    {
        private const string ExamId = "95297ee9-743a-4a1a-a641-2c0bd33e3254";
        private const string SubjectId = "dd13f3d1-d5d3-4d2e-9f20-7524485f7e3b";
        private const string QuestionId = "32515538-e217-4ae6-ab3b-61a44617588d";
        private const string TakeId = "423967e0-d04b-4f7c-a324-e94972d46abc";
        private const string AOptionId = "66bc1977-1758-43d2-a32f-1397777775f2";
        private const string BOptionId = "b650ef7a-3146-4893-a634-4de3e75fa668";
        private const string UserId = "851d1908-0d26-4b4c-9465-584e3aa56aed";
        private const string RoleId = "984528b8-c0dd-4a09-884e-b65d3c97fcef";

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
                TakeExamId = TakeId,
                CheckedOptionId = AOptionId,
                ExamId = ExamId,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();

            Assert.CatchAsync<DbUpdateException>(async () => await service.AddAnswer(model, ExamId));
        }

        [Test]
        public async Task AddAnswerShouldReturnFalseIfTakeExamDoesNotExist()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = QuestionId,
                TakeExamId = Guid.NewGuid().ToString(),
                CheckedOptionId = AOptionId,
                ExamId = ExamId,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();
            var result = await service.AddAnswer(model, ExamId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddAnswerShouldReturnTrueIfCheckedOptionIdEqualsAnsweOptionId()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = QuestionId,
                TakeExamId = TakeId,
                CheckedOptionId = AOptionId,
                ExamId = ExamId,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();
            var result = await service.AddAnswer(model, ExamId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddAnswerShouldReturnTrueIfCheckedOptionIdDoesNotEqualAnswerOptionId()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = QuestionId,
                TakeExamId = TakeId,
                CheckedOptionId = BOptionId,
                ExamId = ExamId,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();
            var result = await service.AddAnswer(model, ExamId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddAnswerShouldReturnTrueIfTakeExamAndQuestionExist()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = QuestionId,
                TakeExamId = TakeId,
                CheckedOptionId = BOptionId,
                ExamId = ExamId,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();
            var result = await service.AddAnswer(model, ExamId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddAnswerShouldReturnFalseIfTakeAnswerAndTakeExamDoNotExist()
        {
            var model = new TakeQuestionVM()
            {
                QuestionId = QuestionId,
                TakeExamId = Guid.NewGuid().ToString(),
                ExamId = ExamId,
                Content = "Some Content"
            };

            var service = this.serviceProvider.GetService<ITakeAnswerService>();
            var result = await service.AddAnswer(model, ExamId);

            Assert.IsFalse(result);
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
                Id = SubjectId.ToGuid(),
                Name = "Български език и литература"
            };

            var exam = new Exam()
            {
                Id = ExamId.ToGuid(),
                Title = "Български език и литература (12 клас)",
                Description = "Тест по БЕЛ за ученици в 12 клас.",
                MaxScore = 2,
                SubjectId = SubjectId.ToGuid(),
                IsActive = true,
            };

            var question = new Question()
            {
                Id = QuestionId.ToGuid(),
                Content = "А, Б, В...?",
                Points = 2,
                ExamId = ExamId.ToGuid(),
                Answers = new List<AnswerOption>()
                {
                    new AnswerOption()
                    {
                        Id = AOptionId.ToGuid(),
                        QuestionId = QuestionId.ToGuid(),
                        Content = "Some Option"
                    },
                    new AnswerOption()
                    {
                        Id = BOptionId.ToGuid(),
                        QuestionId = QuestionId.ToGuid(),
                        Content = "Some Option"
                    }
                },
            };

            var role = new IdentityRole
            {
                Name = "Admin",
                Id = RoleId,
                ConcurrencyStamp = RoleId
            };

            var user = new ApplicationUser
            {
                Id = UserId,
                Email = "user@user.com",
                NormalizedEmail = "USER@USER.COM",
                EmailConfirmed = true,
                FirstName = "User",
                LastName = "User",
                UserName = "user@user.com",
            };

            var take = new TakeExam()
            {
                Id = TakeId.ToGuid(),
                UserId = UserId,
                ExamId = ExamId.ToGuid(),
                TakeAnswers = new List<TakeAnswer>()
                {
                    new TakeAnswer()
                    {
                        TakeExamId = TakeId.ToGuid(),
                        QuestionId = QuestionId.ToGuid(),
                        AnswerOptionId = AOptionId.ToGuid()
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
