using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Subject;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Core.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IApplicationDbRepository repository;

        public SubjectService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Activate(Guid id)
        {
            bool result = false;
            var subject = await this.repository.GetByIdAsync<Subject>(id);

            if (subject != null)
            {
                subject.IsActive = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> AddSubject(SubjectVM model)
        {
            var subject = new Subject()
            {
                Name = model.Name,
            };

            await this.repository.AddAsync(subject);
            await this.repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Deactivate(Guid id)
        {
            bool result = false;
            var subject = await this.repository.GetByIdAsync<Subject>(id);

            if (subject != null)
            {
                subject.IsActive = false;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> Delete(Guid id)
        {
            bool result = false;
            var subject = await this.repository.GetByIdAsync<Subject>(id);

            if (subject != null)
            {
                await this.repository.DeleteAsync<Subject>(subject.Id);
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> Edit(SubjectVM model)
        {
            bool result = false;
            var subject = await this.repository.GetByIdAsync<Subject>(Guid.Parse(model.Id));

            if (subject != null)
            {
                subject.Name = model.Name;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<SubjectVM>> GetActiveSubjects()
        {
            return await this.repository.All<Subject>()
                .Where(s => s.IsActive)
                .Select(s => new SubjectVM()
                {
                    Id = s.Id.ToString(),
                    Name = s.Name,
                    IsActive = s.IsActive,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SubjectVM>> GetAllSubjects()
        {
            return await this.repository.All<Subject>()
                .Select(s => new SubjectVM()
                {
                    Id = s.Id.ToString(),
                    Name = s.Name,
                    IsActive = s.IsActive,
                })
                .ToListAsync();
        }

        public async Task<Subject> GetSubjectById(Guid id)
        {
            return await this.repository.GetByIdAsync<Subject>(id);
        }

        public async Task<SubjectVM> GetSubjectForEdit(Guid id)
        {
            var subject = await this.repository.GetByIdAsync<Subject>(id);

            return new SubjectVM()
            {
                Id = subject.Id.ToString(),
                Name = subject.Name
            };
        }
    }
}
