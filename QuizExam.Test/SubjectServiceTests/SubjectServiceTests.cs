using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.Subject;
using QuizExam.Core.Services;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Test.SubjectServiceTests
{
    public class SubjectServiceTests
    {
        private const string SubjectId = "dd13f3d1-d5d3-4d2e-9f20-7524485f7e3b";

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
                .AddSingleton<ISubjectService, SubjectService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void CreateMustThrowExceptionIfNameIsNull()
        {
            var model = new NewSubjectVM();

            var service = this.serviceProvider.GetService<ISubjectService>();
            Assert.CatchAsync<DbUpdateException>(async () => await service.CreateAsync(model), "NOT NULL constraint failed: Subjects.Name");
        }

        [Test]
        public async Task ActivateNotExistingSubjectMustReturnFalse()
        {
            var service = serviceProvider.GetService<ISubjectService>();
            bool result = await service.ActivateAsync(Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ActivateExistingSubjectMustReturnTrue()
        {
            var service = serviceProvider.GetService<ISubjectService>();
            bool result = await service.ActivateAsync(SubjectId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeactivateNotExistingSubjectMustReturnFalse()
        {
            var service = serviceProvider.GetService<ISubjectService>();
            bool result = await service.DeactivateAsync(Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeactivateExistingSubjectMustReturnTrue()
        {
            var service = serviceProvider.GetService<ISubjectService>();
            bool result = await service.DeactivateAsync(SubjectId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task EditNotExistingSubjectMustReturnFalse()
        {
            var model = new NewSubjectVM()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Some Name"
            };

            var service = serviceProvider.GetService<ISubjectService>();
            bool result = await service.EditAsync(model);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditExistingSubjectMustReturnTrue()
        {
            var model = new NewSubjectVM()
            {
                Id = SubjectId,
                Name = "Some Name"
            };

            var service = serviceProvider.GetService<ISubjectService>();
            bool result = await service.EditAsync(model);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetSubjectForEditMethodMustReturnEmptyModelWhenSubjectDoesNotExist()
        {
            var service = serviceProvider.GetService<ISubjectService>();
            var result = await service.GetSubjectForEditAsync(Guid.NewGuid().ToString());

            Assert.That(result, Is.TypeOf<NewSubjectVM>());
            Assert.IsNull(result.Id);
        }

        [Test]
        public async Task GetSubjectForEditMethodMustReturnModelWhenExamExists()
        {
            var service = serviceProvider.GetService<ISubjectService>();
            var result = await service.GetSubjectForEditAsync(SubjectId);

            Assert.That(result, Is.TypeOf<NewSubjectVM>());
            Assert.IsNotNull(result.Id);
        }

        [Test]
        public async Task GetAllSubjectsMustReturnSubjectsListModel()
        {
            var service = serviceProvider.GetService<ISubjectService>();
            var result = await service.GetAllSubjectsAsync();

            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<List<SubjectVM>>());
            Assert.That(result.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task GetActiveSubjectsMustReturnSubjectsListModel()
        {
            var service = serviceProvider.GetService<ISubjectService>();
            var result = await service.GetActiveSubjectsAsync();

            Assert.IsNotNull(result);
            Assert.That(result, Is.TypeOf<List<SubjectVM>>());
            Assert.That(result.Count(), Is.EqualTo(5));
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

            await repo.AddAsync(subject);
            await repo.SaveChangesAsync();
        }
    }
}
