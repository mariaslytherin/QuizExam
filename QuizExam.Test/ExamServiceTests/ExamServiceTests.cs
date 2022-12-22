using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.Exam;
using QuizExam.Core.Services;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Test.ExamServiceTests
{
    public class ExamServiceTests
    {
        private const string ExamId_Math = "93b3c9bb-93f9-4755-827a-b0bf9964270d";
        private const string ExamId_Bg = "95297ee9-743a-4a1a-a641-2c0bd33e3254";
        private const string SubjectId_Math = "b09225a6-6ff5-4aff-b073-492be59c4ff6";
        private const string SubjectId_Bg = "dd13f3d1-d5d3-4d2e-9f20-7524485f7e3b";

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
            var exam = new Exam()
            {
                Title = "Some Title Here",
                Description = "Some Description Here",
                MaxScore = 50,
                SubjectId = Guid.NewGuid(),
            };
            
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Activate(exam.Id.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ActivateExistingExamMustReturnTrue()
        {
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Activate(ExamId_Math);

            Assert.IsTrue(result);
        }

        [Test]
        public void CreationOfExamMustThrowExceptionWhenTitleIsNull()
        {
            var model = new NewExamVM()
            {
                Description = "Some Description",
                MaxScore = 0,
                SubjectId = SubjectId_Math
            };

            var service = serviceProvider.GetService<IExamService>();
            Assert.CatchAsync<DbUpdateException>(async () => await service.Create(model), "NOT NULL constraint failed: Exams.Title");
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
            Assert.CatchAsync<DbUpdateException>(async () => await service.Create(model));
        }

        [Test]
        public async Task DectivationOfNotExistingExamMustReturnFalse()
        {
            var exam = new Exam()
            {
                Title = "Some Title Here",
                Description = "Some Description Here",
                MaxScore = 50,
                SubjectId = Guid.NewGuid(),
            };

            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Deactivate(exam.Id.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DectivationOfExistingExamMustReturnTrue()
        {
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Deactivate(ExamId_Math);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeletionOfNotExistingExamMustReturnFalse()
        {
            var exam = new Exam()
            {
                Title = "Some Title Here",
                Description = "Some Description Here",
                MaxScore = 50,
                SubjectId = Guid.NewGuid(),
            };

            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Delete(exam.Id.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeletionOfExistingExamMustReturnTrue()
        {
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Delete(ExamId_Math);

            Assert.IsTrue(result);
        }

        [Test]
        public void EditMethodMustThrowExceptionIfTitleIsNull()
        {
            var model = new EditExamVM()
            {
                Id = ExamId_Math,
                Description = "Some Description",
                MaxScore = 100,
                SubjectName = "Subject Name",
            };

            var service = serviceProvider.GetService<IExamService>();
            Assert.CatchAsync<DbUpdateException>(async () => await service.Edit(model), "NOT NULL constraint failed: Exams.Title");
        }

        [Test]
        public async Task EditMethodMustReturnFalseIfExamDoesNotExist()
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
            bool result = await service.Edit(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditMethodMustReturnTrueIfExamExists()
        {
            var model = new EditExamVM()
            {
                Id = ExamId_Math,
                Title = "Some Title Here",
                Description = "Some Description Here",
                MaxScore = 50,
                SubjectName = "Some Subject Name",
            };

            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Edit(model);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllExamsMethodMustReturnExamListModel()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetAllExams(1, 10);

            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<ExamListVM>());
            Assert.That(result.Exams.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetExamsForUserMethodMustReturnZeroListOfExams()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamsForUser();

            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<List<ViewExamVM>>());
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetExamForEditMethodMustReturnEmptyModelWhenExamDoesNotExist()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamForEdit(Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<EditExamVM>());
            Assert.IsNull(result.Id);
        }

        [Test]
        public async Task GetExamForEditMethodMustReturnModelWhenExamExists()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamForEdit(ExamId_Math);

            Assert.That(result, Is.TypeOf<EditExamVM>());
            Assert.IsNotNull(result.Id);
        }

        [Test]
        public async Task GetExamForViewMustReturnEmptyModelIfExamDoesNotExist()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamForView(Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<ViewExamVM>());
            Assert.IsNull(result.Id);
        }

        [Test]
        public async Task GetExamForViewMustReturnEmptyQuestionsListWhenExamDoesNotHaveQuestions()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamForView(ExamId_Math);

            Assert.That(result, Is.TypeOf<ViewExamVM>());
            Assert.That(result.Questions.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetExamForViewMustReturnFilledModelIfExamHasQuestions()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamForView(ExamId_Bg);

            Assert.That(result, Is.TypeOf<ViewExamVM>());
            Assert.That(result.Questions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetExamInfoMustReturnEmptyModelIfExamDoesNotExists()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamInfo(Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<ExamVM>());
            Assert.IsNull(result.Id);
        }

        [Test]
        public async Task GetExamInfoMustReturnFilledModelWithZeroQuestionsCount()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamInfo(ExamId_Math);

            Assert.That(result, Is.TypeOf<ExamVM>());
            Assert.IsNotNull(result.Id);
            Assert.That(result.QuestionsCount, Is.EqualTo(0));
        }

        [Test]
        public async Task GetExamInfoMustReturnFilledModelWithQuestionsMoreThanZero()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.GetExamInfo(ExamId_Bg);

            Assert.That(result, Is.TypeOf<ExamVM>());
            Assert.IsNotNull(result.Id);
            Assert.That(result.QuestionsCount, Is.EqualTo(1));
        }

        [Test]
        public async Task CanActivateMethodMustReturnFalseIfExamIdDoesNotExist()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.HasAnyQuestions(Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CanActivateMethodMustReturnFalseIfExamDoesNotHaveQuestions()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.HasAnyQuestions(ExamId_Math);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CanActivateMethodMustReturnTrueIfExamHasQuestions()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.HasAnyQuestions(ExamId_Bg);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task QuestionsPointsSumEqualsMaxScoreMustReturnFalseIfExamDoesNotExist()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.QuestionsPointsSumEqualsMaxScore(Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task QuestionsPointsSumEqualsMaxScoreMustReturnTrueIfPointsAreEqual()
        {
            var service = serviceProvider.GetService<IExamService>();
            var result = await service.QuestionsPointsSumEqualsMaxScore(ExamId_Bg);

            Assert.True(result);
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
                    Id = SubjectId_Math.ToGuid(),
                    Name = "Математика",
                },
                new Subject()
                {
                    Id = SubjectId_Bg.ToGuid(),
                    Name = "Български език и литература"
                }
            };

            var exams = new List<Exam>()
            {
                new Exam()
                {
                    Id = ExamId_Math.ToGuid(),
                    Title = "Математика (12 клас)",
                    Description = "Тест по математика за ученици в 12 клас.",
                    MaxScore = 100,
                    SubjectId = SubjectId_Math.ToGuid(),
                },
                new Exam()
                {
                    Id = ExamId_Bg.ToGuid(),
                    Title = "Български език и литература (12 клас)",
                    Description = "Тест по БЕЛ за ученици в 12 клас.",
                    MaxScore = 2,
                    SubjectId = SubjectId_Bg.ToGuid(),
                    IsActive = true,
                }
            };

            var questionId = "32515538-e217-4ae6-ab3b-61a44617588d";

            var question = new Question()
            {
                Id = questionId.ToGuid(),
                Content = "А, Б, В...?",
                Points = 2,
                ExamId = ExamId_Bg.ToGuid(),
            };

            await repo.AddRangeAsync(subjects);
            await repo.AddRangeAsync(exams);
            await repo.AddAsync(question);
            await repo.SaveChangesAsync();
        }
    }
}
