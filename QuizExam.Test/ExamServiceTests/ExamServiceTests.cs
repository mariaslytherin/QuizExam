using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.Exam;
using QuizExam.Core.Services;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Repositories;
using QuizExam.Test.Constants;

namespace QuizExam.Test.ExamServiceTests
{
    public class ExamServiceTests
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
                .AddSingleton<IExamService, ExamService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public async Task ActivateNotExistingExamMustReturnFalse()
        {
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.ActivateAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ActivateExistingExamMustReturnTrue()
        {
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.ActivateAsync(UniqueIdentifiersTestConstants.ExamId_Math);

            Assert.That(result, Is.True);
        }

        [Test]
        public void CreationOfExamMustThrowExceptionWhenTitleIsNull()
        {
            var model = new NewExamVM()
            {
                Description = "Some Description",
                MaxScore = 0,
                SubjectId = UniqueIdentifiersTestConstants.SubjectId_Math
            };

            var service = serviceProvider.GetService<IExamService>();
            Assert.CatchAsync<DbUpdateException>(async () => await service.CreateAsync(model), "NOT NULL constraint failed: Exams.Title");
        }

        [Test]
        public void CreationOfExamMustThrowExceptionWhenSubjectDoesNotExist()
        {
            var model = new NewExamVM()
            {
                Title = "Some Title",
                Description = "Some Description",
                MaxScore = 0,
                SubjectId = "93b3c9bb-93f9-4755-827a-b0bf9964270d"
            };

            var service = serviceProvider.GetService<IExamService>();
            Assert.CatchAsync<DbUpdateException>(async () => await service.CreateAsync(model));
        }

        [Test]
        public async Task DeactivationOfNotExistingExamMustReturnFalse()
        {
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.DeactivateAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeactivationOfExistingExamMustReturnTrue()
        {
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.DeactivateAsync(UniqueIdentifiersTestConstants.ExamId_Math);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeletionOfNotExistingExamMustReturnFalse()
        {
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.DeleteAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeletionOfExistingExamMustReturnTrue()
        {
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.DeleteAsync(UniqueIdentifiersTestConstants.ExamId_Math);

            Assert.That(result, Is.True);
        }

        [Test]
        public void EditMustThrowExceptionIfTitleIsNull()
        {
            var model = new EditExamVM()
            {
                Id = UniqueIdentifiersTestConstants.ExamId_Math,
                Description = "Some Description",
                MaxScore = 100,
                SubjectName = "Subject Name",
            };

            var service = serviceProvider.GetService<IExamService>();
            Assert.CatchAsync<DbUpdateException>(async () => await service.EditAsync(model), "NOT NULL constraint failed: Exams.Title");
        }

        [Test]
        public async Task EditMustReturnFalseIfExamDoesNotExist()
        {
            var model = new EditExamVM()
            {
                Id = "b09225a6-6ff5-4aff-b073-492be59c4ff6",
                Title = "Some Title Here",
                Description = "Some Description Here",
                MaxScore = 50,
                SubjectName = "Some Subject Name",
            };

            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.EditAsync(model);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task EditMustReturnTrueIfExamExists()
        {
            var model = new EditExamVM()
            {
                Id = UniqueIdentifiersTestConstants.ExamId_Math,
                Title = "Some Title Here",
                Description = "Some Description Here",
                MaxScore = 50,
                SubjectName = "Some Subject Name",
            };

            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.EditAsync(model);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetAllExamsMustReturnExamListModel()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetAllExamsAsync(1, 10);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ExamListVM>());
            Assert.That(result.Exams.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetExamsForUserMustReturnZeroListOfExams()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamsForUserAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<List<ViewExamVM>>());
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetExamForEditMustReturnEmptyModelWhenExamDoesNotExist()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamForEditAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<EditExamVM>());
            Assert.That(result.Id, Is.Null);
        }

        [Test]
        public async Task GetExamForEditMustReturnModelWhenExamExists()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamForEditAsync(UniqueIdentifiersTestConstants.ExamId_Math);

            Assert.That(result, Is.TypeOf<EditExamVM>());
            Assert.That(result.Id, Is.Not.Null);
        }

        [Test]
        public async Task GetExamForViewMustReturnEmptyModelIfExamDoesNotExist()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamForViewAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<ViewExamVM>());
            Assert.That(result.Id, Is.Null);
        }

        [Test]
        public async Task GetExamForViewMustReturnEmptyQuestionsListWhenExamDoesNotHaveQuestions()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamForViewAsync(UniqueIdentifiersTestConstants.ExamId_Math);

            Assert.That(result, Is.TypeOf<ViewExamVM>());
            Assert.That(result.Questions.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetExamForViewMustReturnFilledModelIfExamHasQuestions()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamForViewAsync(UniqueIdentifiersTestConstants.ExamId_Bg);

            Assert.That(result, Is.TypeOf<ViewExamVM>());
            Assert.That(result.Questions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetExamInfoMustReturnEmptyModelIfExamDoesNotExists()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamInfoAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<ExamVM>());
            Assert.That(result.Id, Is.Null);
        }

        [Test]
        public async Task GetExamInfoMustReturnFilledModelWithZeroQuestionsCount()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamInfoAsync(UniqueIdentifiersTestConstants.ExamId_Math);

            Assert.That(result, Is.TypeOf<ExamVM>());
            Assert.That(result.Id, Is.Not.Null);
            Assert.That(result.QuestionsCount, Is.EqualTo(0));
        }

        [Test]
        public async Task GetExamInfoMustReturnFilledModelWithQuestionsMoreThanZero()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamInfoAsync(UniqueIdentifiersTestConstants.ExamId_Bg);

            Assert.That(result, Is.TypeOf<ExamVM>());
            Assert.That(result.Id, Is.Not.Null);
            Assert.That(result.QuestionsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task CanActivateMustReturnFalseIfExamIdDoesNotExist()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.HasAnyQuestionsAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CanActivateMustReturnFalseIfExamDoesNotHaveQuestions()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.HasAnyQuestionsAsync(UniqueIdentifiersTestConstants.ExamId_Math);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CanActivateMustReturnTrueIfExamHasQuestions()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.HasAnyQuestionsAsync(UniqueIdentifiersTestConstants.ExamId_Bg);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task QuestionsPointsSumEqualsMaxScoreMustReturnFalseIfExamDoesNotExist()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.QuestionsPointsSumEqualsMaxScoreAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task QuestionsPointsSumEqualsMaxScoreMustReturnTrueIfPointsAreEqual()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.QuestionsPointsSumEqualsMaxScoreAsync(UniqueIdentifiersTestConstants.ExamId_Bg);

            // TODO
            Assert.That(result, Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {
            var subjects = new List<Subject>()
            {
                new Subject()
                {
                    Id = UniqueIdentifiersTestConstants.SubjectId_Math.ToGuid(),
                    Name = "Математика",
                },
                new Subject()
                {
                    Id = UniqueIdentifiersTestConstants.SubjectId_Bg.ToGuid(),
                    Name = "Български език и литература"
                }
            };

            var exams = new List<Exam>()
            {
                new Exam()
                {
                    Id = UniqueIdentifiersTestConstants.ExamId_Math.ToGuid(),
                    Title = "Математика (12 клас)",
                    Description = "Тест по математика за ученици в 12 клас.",
                    MaxScore = 100,
                    SubjectId = UniqueIdentifiersTestConstants.SubjectId_Math.ToGuid(),
                },
                new Exam()
                {
                    Id = UniqueIdentifiersTestConstants.ExamId_Bg.ToGuid(),
                    Title = "Български език и литература (12 клас)",
                    Description = "Тест по БЕЛ за ученици в 12 клас.",
                    MaxScore = 2,
                    SubjectId = UniqueIdentifiersTestConstants.SubjectId_Bg.ToGuid(),
                    IsActive = true,
                }
            };

            var question = new Question()
            {
                Id = UniqueIdentifiersTestConstants.QuestionId.ToGuid(),
                Content = "А, Б, В...?",
                Points = 2,
                ExamId = UniqueIdentifiersTestConstants.ExamId_Bg.ToGuid(),
            };

            await repo.AddRangeAsync(subjects);
            await repo.AddRangeAsync(exams);
            await repo.AddAsync(question);
            await repo.SaveChangesAsync();
        }
    }
}
