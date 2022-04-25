using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.Question;
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

        public async Task<bool> Activate(Guid id)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(id);

            if (exam != null)
            {
                exam.IsActive = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> Create(NewExamVM model)
        {
            var exam = new Exam()
            {
                Title = model.Title,
                Description = model.Description,
                MaxScore = model.MaxScore,
                SubjectId = Guid.Parse(model.SubjectId),
            };

            await this.repository.AddAsync(exam);
            await this.repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Deactivate(Guid id)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(id);

            if (exam != null)
            {
                exam.IsActive = false;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> Delete(Guid id)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(id);

            if (exam != null)
            {
                exam.IsDeleted = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> Edit(EditExamVM model)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(Guid.Parse(model.Id));

            if (exam != null)
            {
                exam.Title = model.Title;
                exam.Description = model.Description;
                exam.MaxScore = model.MaxScore;
                exam.ModifyDate = DateTime.Today;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<ExamListVM>> GetAllExams()
        {
            return await this.repository.All<Exam>()
                .Where(e => !e.IsDeleted)
                .Join(this.repository.All<Subject>(),
                      e => e.SubjectId, s => s.Id,
                (e, s) => new ExamListVM()
                {
                    Id = e.Id.ToString(),
                    Title = e.Title,
                    SubjectName = s.Name,
                    CreateDate = e.CreateDate,
                    IsActive = e.IsActive ? "Yes" : "No",
                }).ToListAsync();
        }

        public async Task<Exam> GetExamById(Guid id)
        {
            return await this.repository.GetByIdAsync<Exam>(id);
        }

        public async Task<EditExamVM> GetExamForEdit(Guid id)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(id);
            var subject = await this.repository.GetByIdAsync<Subject>(exam.SubjectId);

            return new EditExamVM()
            {
                Id = exam.Id.ToString(),
                Title = exam.Title,
                Description = exam.Description,
                MaxScore = exam.MaxScore,
                SubjectName = subject.Name,
            };
        }

        public async Task<ViewExamVM> GetExamForView(Guid id)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(id);
            var subject = await this.repository.GetByIdAsync<Subject>(exam.SubjectId);
            var questions = this.repository.All<Question>().Where(q => q.ExamId == id && !q.IsDeleted)
                .Select(q => new QuestionExamVM
                {
                    Id = q.Id.ToString(),
                    Content = q.Content,
                    Points = q.Points,
                    AnswerOptions = this.repository
                        .All<AnswerOption>()
                        .Where(a => a.QuestionId == q.Id && !a.IsDeleted)
                        .Select(a => new AnswerOptionVM
                        {
                            Content = a.Content,
                            IsCorrect = a.IsCorrect,
                        })
                        .DefaultIfEmpty()
                        .ToList()
                })
                .DefaultIfEmpty()
                .ToList();

            return new ViewExamVM
            {
                Id = exam.Id.ToString(),
                Title= exam.Title,
                Description= exam.Description,
                SubjectName = subject.Name,
                Questions = questions,
            };
        }
    }
}
