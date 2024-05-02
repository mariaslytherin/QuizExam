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
using QuizExam.Test.Constants;

namespace QuizExam.Test.QuestionServiceTests
{
    public class QuestionServiceTests
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
                ExamId = UniqueIdentifiersTestConstants.ExamId_Bg,
                Content = "Some Content In Here",
                Points = 1,
                Rule = "Some Rule Here"
            };

            var service = this.serviceProvider.GetService<IQuestionService>();
            var questionId = await service.CreateAsync(model);

            //Assert.That(questionId, Is.Not.Null);
            Assert.That(questionId, Is.TypeOf<Guid>());
        }

        [Test]
        public async Task DeletionOfNotExistingQuestionMustReturnFalse()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            bool result = await service.DeleteAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeletionOfExistingQuetionMustReturnTrue()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            bool result = await service.DeleteAsync(UniqueIdentifiersTestConstants.QuestionId);

            Assert.That(result, Is.True);
        }

        [Test]
        public void EditMustThrowExceptionIfContentIsNull()
        {
            var model = new EditQuestionVM()
            {
                Id = UniqueIdentifiersTestConstants.QuestionId,
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
                QuestionContent = "Some Content",
                Rule = "Some Rule",
                Points = 2
            };

            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.EditAsync(model);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task EditMustReturnTrueIfQuestionIsEditedSuccessfuly()
        {
            var model = new EditQuestionVM()
            {
                Id = UniqueIdentifiersTestConstants.QuestionId,
                QuestionContent = "Some Content",
                Rule = "Some Rule",
                Points = 2
            };

            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.EditAsync(model);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetQuestionByIdMustReturnNullIfQuestionDoesNotExist()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetQuestionByIdAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetQuestionByIdMustReturnObjectIfQuestionExists()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetQuestionByIdAsync(UniqueIdentifiersTestConstants.QuestionId);

            Assert.That(result, Is.TypeOf<Question>());
        }

        [Test]
        public async Task GetQuestionForEditMustReturnEmptyModelIfQuestionDoesNotExist()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetQuestionForEditAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<EditQuestionVM>());
            Assert.That(result.Id, Is.Null);
        }

        [Test]
        public async Task GetQuestionForEditMustReturnModelIfQuestionExists()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetQuestionForEditAsync(UniqueIdentifiersTestConstants.QuestionId);

            Assert.That(result, Is.TypeOf<EditQuestionVM>());
            Assert.That(result.Id, Is.Not.Null);
        }

        [Test]
        public async Task GetNextQuestionMustReturnEmptyModelIfExamIdIsNull()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetNextQuestionAsync(Guid.NewGuid().ToString(), UniqueIdentifiersTestConstants.TakeId, 0);

            Assert.That(result, Is.TypeOf<TakeQuestionVM>());
            Assert.That(result.QuestionId, Is.Null);
        }

        [Test]
        public void GetNextQuestionMustThrowExceptionIfOrderIsGreaterThanQuestionsCount()
        {
            var service = serviceProvider.GetService<IQuestionService>();

            Assert.CatchAsync<ArgumentOutOfRangeException>
                (async () => await service.GetNextQuestionAsync(UniqueIdentifiersTestConstants.ExamId_Bg, UniqueIdentifiersTestConstants.TakeId, 5));
        }

        [Test]
        public async Task GetNextQuestionMustReturnModelIfDataIsCorrect()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetNextQuestionAsync(UniqueIdentifiersTestConstants.ExamId_Bg, UniqueIdentifiersTestConstants.TakeId, 0);

            Assert.That(result, Is.TypeOf<TakeQuestionVM>());
            Assert.That(result.QuestionId, Is.Not.Null);
        }

        [Test]
        public async Task GetPreviousQuestionMustReturnEmptyModelIfExamIdIsNull()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetPreviousQuestionAsync(Guid.NewGuid().ToString(), UniqueIdentifiersTestConstants.TakeId, 0);

            Assert.That(result, Is.TypeOf<TakeQuestionVM>());
            Assert.That(result.QuestionId, Is.Null);
        }

        [Test]
        public void GetPreviousQuestionMustThrowExceptionIfOrderIsGreaterThanQuestionsCount()
        {
            var service = serviceProvider.GetService<IQuestionService>();

            Assert.CatchAsync<ArgumentOutOfRangeException>
                (async () => await service.GetPreviousQuestionAsync(UniqueIdentifiersTestConstants.ExamId_Bg, UniqueIdentifiersTestConstants.TakeId, 5));
        }

        [Test]
        public async Task GetNextPreviousMustReturnModelIfDataIsCorrect()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.GetPreviousQuestionAsync(UniqueIdentifiersTestConstants.ExamId_Bg, UniqueIdentifiersTestConstants.TakeId, 0);

            Assert.That(result, Is.TypeOf<TakeQuestionVM>());
            Assert.That(result.QuestionId, Is.Not.Null);
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
            var result = service.GetLastNotTakenQuestionOrder(UniqueIdentifiersTestConstants.TakeId);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task HasEnoughAnswerOptionsMustReturnFalseIfOptionsAreNotEnough()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.HasEnoughAnswerOptionsAsync(UniqueIdentifiersTestConstants.QuestionId);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task HasEnoughAnswerOptionsMustReturnFalseIfQuestionDoesNotExist()
        {
            var service = serviceProvider.GetService<IQuestionService>();
            var result = await service.HasEnoughAnswerOptionsAsync(Guid.NewGuid().ToString());

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
