using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Exam;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Core.Services
{
    public class ExamService : IExamService
    {
        private readonly IApplicationDbRepository repository;

        public ExamService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CreateExam(NewExamVM model)
        {
            var exam = new Exam()
            {
                Title = model.Title,
                Description = model.Description,
                MaxScore = model.MaxScore,
                SubjectId = Guid.Parse(model.SubjectId),
            };

            await repository.AddAsync(exam);
            await repository.SaveChangesAsync();

            return true;
        }
    }
}
