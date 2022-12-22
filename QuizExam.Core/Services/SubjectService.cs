using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
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

        public async Task<bool> ActivateAsync(string id)
        {
            bool result = false;
            var subject = await this.repository.GetByIdAsync<Subject>(id.ToGuid());

            if (subject != null)
            {
                subject.IsActive = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task CreateAsync(NewSubjectVM model)
        {
            var subject = new Subject()
            {
                Name = model.Name,
            };

            await this.repository.AddAsync(subject);
            await this.repository.SaveChangesAsync();
        }

        public async Task<bool> DeactivateAsync(string id)
        {
            bool result = false;
            var subject = await this.repository.GetByIdAsync<Subject>(id.ToGuid());

            if (subject != null)
            {
                subject.IsActive = false;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> EditAsync(NewSubjectVM model)
        {
            bool result = false;
            var subject = await this.repository.GetByIdAsync<Subject>(model.Id.ToGuid());

            if (subject != null)
            {
                subject.Name = model.Name;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<SubjectVM>> GetActiveSubjectsAsync()
        {
            return await this.repository.All<Subject>()
                .Where(s => s.IsActive)
                .Select(s => new SubjectVM()
                {
                    Id = s.Id.ToString(),
                    Name = s.Name
                })
                .ToListAsync();
        }

        public async Task<IList<SubjectVM>> GetAllSubjectsAsync()
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

        public async Task<NewSubjectVM> GetSubjectForEditAsync(string id)
        {
            var subject = await this.repository.GetByIdAsync<Subject>(id.ToGuid());

            if (subject != null)
            {
                return new NewSubjectVM()
                {
                    Id = subject.Id.ToString(),
                    Name = subject.Name
                };
            }

            return new NewSubjectVM();
        }
    }
}
