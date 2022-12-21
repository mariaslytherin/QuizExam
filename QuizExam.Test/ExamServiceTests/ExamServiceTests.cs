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
        public async Task ActivationOfNotExistingExamMustReturnFalse()
        {
            var exam = new Exam()
            {
                Title = "Some Title Here",
                Description = "Some Description Here",
                MaxScore = 50,
                SubjectId = Guid.NewGuid(),
            };
            
            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Activate(exam.Id);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task ActivationOfExistingExamMustReturnTrue()
        {
            var examId = "93b3c9bb-93f9-4755-827a-b0bf9964270d";

            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Activate(examId.ToGuid());
            Assert.IsTrue(result);
        }

        [Test]
        public void CreationOfExamMustThrowExceptionWhenTitleIsNull()
        {
            var model = new NewExamVM()
            {
                Description = "Some Description",
                MaxScore = 0,
                SubjectId = "b09225a6-6ff5-4aff-b073-492be59c4ff6"
            };

            var service = serviceProvider.GetService<IExamService>();
            Assert.CatchAsync<DbUpdateException>(async () => await service.Create(model), "NOT NULL constraint failed: Exams.Title");
        }

        [Test]
        public void CreationOfExamMustThrowExceptionWhenSubjectIdIsNull()
        {
            var model = new NewExamVM()
            {
                Title = "Some Title",
                Description = "Some Description",
                MaxScore = 0,
            };

            var service = serviceProvider.GetService<IExamService>();
            Assert.CatchAsync<NullReferenceException>(async () => await service.Create(model), "Object reference not set to an instance of an object.");
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
            bool result = await service.Deactivate(exam.Id);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DectivationOfExistingExamMustReturnTrue()
        {
            var examId = "93b3c9bb-93f9-4755-827a-b0bf9964270d";

            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Deactivate(examId.ToGuid());
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
            bool result = await service.Delete(exam.Id);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeletionOfExistingExamMustReturnTrue()
        {
            var examId = "93b3c9bb-93f9-4755-827a-b0bf9964270d";

            var service = serviceProvider.GetService<IExamService>();
            bool result = await service.Delete(examId.ToGuid());
            Assert.IsTrue(result);
        }

        [Test]
        public void EditMethodMustThrowExceptionIfExamIdIsNull()
        {
            var model = new EditExamVM()
            {
                Title = "Some Title Here",
                Description = "Some Description Here",
                MaxScore = 50,
                SubjectName = "Some Subject Name",
            };

            var service = serviceProvider.GetService<IExamService>();
            Assert.CatchAsync<NullReferenceException>(async () => await service.Edit(model), "Object reference not set to an instance of an object.");
        }

        [Test]
        public void EditMethodMustThrowExceptionIfTitleIsNull()
        {
            var model = new EditExamVM()
            {
                Id = "93b3c9bb-93f9-4755-827a-b0bf9964270d",
                Description = "Тест по математика за ученици в 12 клас.",
                MaxScore = 100,
                SubjectName = "Математика",
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
                Id = "93b3c9bb-93f9-4755-827a-b0bf9964270d",
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
        public async Task GetAllExamsMethodMustReturnExamListVM()
        {
            var service = serviceProvider.GetService<IExamService>();
            ExamListVM result = await service.GetAllExams(1, 10);
            Assert.That(result, Is.TypeOf<ExamListVM>());
        }

        [Test]
        public async Task GetExamsForUserMethodMustReturnListOfExams()
        {
            var service = serviceProvider.GetService<IExamService>();
            List<ViewExamVM> result = await service.GetExamsForUser();
            Assert.That(result, Is.TypeOf<ExamListVM>());
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {
            var subjectId = "b09225a6-6ff5-4aff-b073-492be59c4ff6";

            var subject = new Subject()
            {
                Id = subjectId.ToGuid(),
                Name = "Математика",
            };

            await repo.AddAsync(subject);

            var examId = "93b3c9bb-93f9-4755-827a-b0bf9964270d";

            var exam = new Exam()
            {
                Id = examId.ToGuid(),
                Title = "Математика (12 клас)",
                Description = "Тест по математика за ученици в 12 клас.",
                MaxScore = 100,
                SubjectId = subject.Id,
            };

            await repo.AddAsync(exam);
            await repo.SaveChangesAsync();
        }
    }
}
