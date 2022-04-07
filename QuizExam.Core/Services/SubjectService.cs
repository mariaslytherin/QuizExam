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

        public async Task AddSubject(SubjectVM model)
        {
            await repository.AddAsync(model.Name);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<SubjectVM>> GetAllSubjects()
        {
            return await this.repository.All<Subject>()
                .Select(s => new SubjectVM()
                {
                    Id = s.Id.ToString(),
                    Name = s.Name
                })
                .ToListAsync();
        }
    }
}
