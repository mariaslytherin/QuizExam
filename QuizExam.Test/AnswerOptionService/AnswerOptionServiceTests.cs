using Microsoft.Extensions.DependencyInjection;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Repositories;
using QuizExam.Test.Constants;

namespace QuizExam.Test.AnswerOptionService
{
    public class AnswerOptionServiceTests
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
                .AddSingleton<IAnswerOptionService, Core.Services.AnswerOptionService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public async Task CreateMustReturnFalseIfQuestionDoesNotExist()
        {
            var model = new NewAnswerOptionVM()
            {
                Content = "Some Content In Here",
            };

            var service = this.serviceProvider.GetService<IAnswerOptionService>();
            var result = await service.CreateAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateMustReturnTrueIfCreateSucceded()
        {
            var model = new NewAnswerOptionVM()
            {
                QuestionId = UniqueIdentifiersTestConstants.QuestionId,
                Content = "Some Content In Here",
            };

            var service = this.serviceProvider.GetService<IAnswerOptionService>();
            var result = await service.CreateAsync(model);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteMustReturnFalseIfOptionDoesNotExist()
        {
            var service = this.serviceProvider.GetService<IAnswerOptionService>();
            var result = await service.DeleteAsync(Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteMustReturnTrueIfDeleteSucceded()
        {
            var service = this.serviceProvider.GetService<IAnswerOptionService>();
            var result = await service.DeleteAsync(UniqueIdentifiersTestConstants.AOptionId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetOptionsMustReturnEmptyCollectionIfQuestionDoesNotExist()
        {
            var service = this.serviceProvider.GetService<IAnswerOptionService>();
            var result = await service.GetOptionsAsync(Guid.NewGuid().ToString());

            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetOptionsMustReturnTrueIfDeleteSucceded()
        {
            var service = this.serviceProvider.GetService<IAnswerOptionService>();
            var result = await service.GetOptionsAsync(UniqueIdentifiersTestConstants.QuestionId);

            Assert.IsNotEmpty(result);
        }

        [Test]
        public void SetCorrectAnswerMustThrowExceptionIfAnyOptionIsNull()
        {
            var model = new SetCorrectAnswerVM()
            {
                CorrectAnswerId = Guid.NewGuid().ToString(),
                Options = new List<AnswerOptionVM>()
                {
                    new AnswerOptionVM()
                    {
                        Id = null
                    }
                },
                QuestionId = Guid.NewGuid().ToString(),
            };

            var service = this.serviceProvider.GetService<IAnswerOptionService>();

            Assert.CatchAsync<NullReferenceException>(async () => await service.SetCorrectAnswerAsync(model));
        }

        [Test]
        public void SetCorrectAnswerMustThrowExceptionIfCorrectAnswerIdIsNull()
        {
            var model = new SetCorrectAnswerVM()
            {
                Options = new List<AnswerOptionVM>()
                {
                    new AnswerOptionVM()
                    {
                        Id = null
                    }
                },
                QuestionId = Guid.NewGuid().ToString(),
            };

            var service = this.serviceProvider.GetService<IAnswerOptionService>();

            Assert.CatchAsync<NullReferenceException>(async () => await service.SetCorrectAnswerAsync(model));
        }

        [Test]
        public async Task SetCorrectAnswerMustReturnFalseIfCorrectAnswerOptionDoesNotExist()
        {
            var model = new SetCorrectAnswerVM()
            {
                CorrectAnswerId = Guid.NewGuid().ToString(),
                Options = new List<AnswerOptionVM>()
                {
                    new AnswerOptionVM()
                    {
                        Id = UniqueIdentifiersTestConstants.AOptionId,
                        IsCorrect = true,
                    }
                },
                QuestionId = Guid.NewGuid().ToString(),
            };

            var service = this.serviceProvider.GetService<IAnswerOptionService>();
            var result = await service.SetCorrectAnswerAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SetCorrectAnswerMustReturnTrueIfModelIsFilledCorrect()
        {
            var model = new SetCorrectAnswerVM()
            {
                CorrectAnswerId = UniqueIdentifiersTestConstants.AOptionId,
                Options = new List<AnswerOptionVM>()
                {
                    new AnswerOptionVM()
                    {
                        Id = UniqueIdentifiersTestConstants.AOptionId,
                        IsCorrect = true,
                    }
                },
                QuestionId = Guid.NewGuid().ToString(),
            };

            var service = this.serviceProvider.GetService<IAnswerOptionService>();
            var result = await service.SetCorrectAnswerAsync(model);

            Assert.IsTrue(result);
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
                SubjectId = subject.Id,
                IsActive = true,
                Questions = new List<Question>()
                {
                    new Question()
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
                                Content = "Г, Д, Е...",
                                QuestionId = UniqueIdentifiersTestConstants.QuestionId.ToGuid()
                            }
                        }
                    }
                }
            };

            await repo.AddAsync(subject);
            await repo.AddAsync(exam);
            await repo.SaveChangesAsync();
        }
    }
}
