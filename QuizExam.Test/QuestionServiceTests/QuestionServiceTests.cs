using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.Question;
using QuizExam.Core.Models.TakeQuestion;
using QuizExam.Core.Services;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Identity;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Test.QuestionServiceTests
{
    public class QuestionServiceTests
    {
        private const string ExamId_Bg = "95297ee9-743a-4a1a-a641-2c0bd33e3254";
        private const string SubjectId_Bg = "dd13f3d1-d5d3-4d2e-9f20-7524485f7e3b";
        private const string QuestionId = "32515538-e217-4ae6-ab3b-61a44617588d";
        private const string TakeId = "423967e0-d04b-4f7c-a324-e94972d46abc";
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
                .AddSingleton<IQuestionService, QuestionService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public async Task CreateQuestionMustReturnEmptyGuidIfExamDoesNotExist()
        {
            var model = new NewQuestionVM()
            {
                ExamId = Guid.NewGuid().ToString(),
                Content = "Some Content In Here",
                Points = 1,
                Rule = "Some Rule Here"
            };

            var service = this.serviceProvider.GetService<IQuestionService>();
            var questionId = await service.CreateAsync(model);

            Assert.That(questionId, Is.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task CreateQuestionMustReturnGuidIfCreationSucceded()
        {
            var model = new NewQuestionVM()
            {
                ExamId = ExamId_Bg,
                Content = "Some Content In Here",
                Points = 1,
                Rule = "Some Rule Here"
            };

            var service = this.serviceProvider.GetService<IQuestionService>();
            var questionId = await service.CreateAsync(model);

            Assert.IsNotNull(questionId);
            Assert.That(questionId, Is.TypeOf<Guid>());
        }

        [Test]
        public async Task DeletionOfNotExistingQuestionMustReturnFalse()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            bool result = await service.DeleteAsync(Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeletionOfExistingQuetionMustReturnTrue()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            bool result = await service.DeleteAsync(QuestionId);

            Assert.IsTrue(result);
        }

        [Test]
        public void EditMustThrowExceptionIfContentIsNull()
        {
            var model = new EditQuestionVM()
            {
                Id = QuestionId,
                Rule = "Some Rule",
                Points = 2,
            };

            var service = serviceProvider.GetService<IQuestionService>();
            Assert.CatchAsync<DbUpdateException>(async () => await service.EditAsync(model), "NOT NULL constraint failed: Questions.Content");
        }

        [Test]
        public async Task EditMustReturnFalseIfQuestionDoesNotExist()
        {
            var model = new EditQuestionVM()
            {
                Id = Guid.NewGuid().ToString(),
                Content = "Some Content",
                Rule = "Some Rule",
                Points = 2
            };

            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.EditAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditMustReturnTrueIfQuestionIsEditedSuccessfuly()
        {
            var model = new EditQuestionVM()
            {
                Id = QuestionId,
                Content = "Some Content",
                Rule = "Some Rule",
                Points = 2
            };

            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.EditAsync(model);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetQuestionByIdMustReturnNullIfQuestionDoesNotExist()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetQuestionByIdAsync(Guid.NewGuid().ToString());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetQuestionByIdMustReturnObjectIfQuestionExists()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetQuestionByIdAsync(QuestionId);

            Assert.That(result, Is.TypeOf<Question>());
        }

        [Test]
        public async Task GetQuestionForEditMustReturnEmptyModelIfQuestionDoesNotExist()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetQuestionForEditAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<EditQuestionVM>());
            Assert.IsNull(result.Id);
        }

        [Test]
        public async Task GetQuestionForEditMustReturnModelIfQuestionExists()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetQuestionForEditAsync(QuestionId);

            Assert.That(result, Is.TypeOf<EditQuestionVM>());
            Assert.IsNotNull(result.Id);
        }

        [Test]
        public async Task GetNextQuestionMustReturnEmptyModelIfExamIdIsNull()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetNextQuestionAsync(Guid.NewGuid().ToString(), TakeId, 0);

            Assert.That(result, Is.TypeOf<TakeQuestionVM>());
            Assert.IsNull(result.QuestionId);
        }

        [Test]
        public void GetNextQuestionMustThrowExceptionIfOrderIsGreaterThanQuestionsCount()
        {
            var service = serviceProvider.GetService<IQuestionService>();

            Assert.CatchAsync<ArgumentOutOfRangeException>(async () => await service.GetNextQuestionAsync(ExamId_Bg, TakeId, 5));
        }

        [Test]
        public async Task GetNextQuestionMustReturnModelIfDataIsCorrect()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetNextQuestionAsync(ExamId_Bg, TakeId, 0);

            Assert.That(result, Is.TypeOf<TakeQuestionVM>());
            Assert.IsNotNull(result.QuestionId);
        }

        [Test]
        public async Task GetPreviousQuestionMustReturnEmptyModelIfExamIdIsNull()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetPreviousQuestionAsync(Guid.NewGuid().ToString(), TakeId, 0);

            Assert.That(result, Is.TypeOf<TakeQuestionVM>());
            Assert.IsNull(result.QuestionId);
        }

        [Test]
        public void GetPreviousQuestionMustThrowExceptionIfOrderIsGreaterThanQuestionsCount()
        {
            var service = serviceProvider.GetService<IQuestionService>();

            Assert.CatchAsync<ArgumentOutOfRangeException>(async () => await service.GetPreviousQuestionAsync(ExamId_Bg, TakeId, 5));
        }

        [Test]
        public async Task GetNextPreviousMustReturnModelIfDataIsCorrect()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetPreviousQuestionAsync(ExamId_Bg, TakeId, 0);

            Assert.That(result, Is.TypeOf<TakeQuestionVM>());
            Assert.IsNotNull(result.QuestionId);
        }


        [Test]
        public void GetLastNotTakenQuestionOrderMustReturnZeroIfTakeExamDoesNotExist()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = service.GetLastNotTakenQuestionOrder(Guid.NewGuid().ToString());

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void GetLastNotTakenQuestionOrderMustReturnTakenQuestionsCountIfTakeExamHasAny()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = service.GetLastNotTakenQuestionOrder(TakeId);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task HasEnoughAnswerOptionsMustReturnFalseIfOptionsAreNotEnough()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.HasEnoughAnswerOptions(QuestionId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task HasEnoughAnswerOptionsMustReturnFalseIfQuestionDoesNotExist()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.HasEnoughAnswerOptions(Guid.NewGuid().ToString());

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
                Id = SubjectId_Bg.ToGuid(),
                Name = "Български език и литература"
            };

            var exam = new Exam()
            {
                Id = ExamId_Bg.ToGuid(),
                Title = "Български език и литература (12 клас)",
                Description = "Тест по БЕЛ за ученици в 12 клас.",
                MaxScore = 2,
                SubjectId = SubjectId_Bg.ToGuid(),
                IsActive = true,
            };

            var optionId = "66bc1977-1758-43d2-a32f-1397777775f2";

            var question = new Question()
            {
                Id = QuestionId.ToGuid(),
                Content = "А, Б, В...?",
                Points = 2,
                ExamId = ExamId_Bg.ToGuid(),
                Answers = new List<AnswerOption>()
                {
                    new AnswerOption()
                    {
                        Id = optionId.ToGuid(),
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
                ExamId = ExamId_Bg.ToGuid(),
                TakeAnswers = new List<TakeAnswer>()
                {
                    new TakeAnswer()
                    {
                        TakeExamId = TakeId.ToGuid(),
                        QuestionId = QuestionId.ToGuid(),
                        AnswerOptionId = optionId.ToGuid()
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
