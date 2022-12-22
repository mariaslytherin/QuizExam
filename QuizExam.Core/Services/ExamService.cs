using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
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

        public async Task<bool> ActivateAsync(string id)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(id.ToGuid());

            if (exam != null)
            {
                exam.IsActive = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task CreateAsync(NewExamVM model)
        {
            var exam = new Exam()
            {
                Title = model.Title,
                Description = model.Description,
                MaxScore = model.MaxScore,
                SubjectId = model.SubjectId.ToGuid(),
            };

            await this.repository.AddAsync(exam);
            await this.repository.SaveChangesAsync();
        }

        public async Task<bool> DeactivateAsync(string id)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(id.ToGuid());

            if (exam != null)
            {
                exam.IsActive = false;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(id.ToGuid());

            if (exam != null)
            {
                exam.IsDeleted = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> EditAsync(EditExamVM model)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(model.Id.ToGuid());

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

        public async Task<ExamListVM> GetAllExamsAsync(int? page, int? size)
        {
            var exams = await this.repository.All<Exam>()
                .Where(e => !e.IsDeleted)
                .Join(this.repository.All<Subject>(),
                      e => e.SubjectId,
                      s => s.Id,
                     (e, s) => new ViewExamVM()
                     {
                         Id = e.Id.ToString(),
                         Title = e.Title,
                         SubjectName = s.Name,
                         Description = e.Description,
                         CreateDate = e.CreateDate.ToDateOnlyString(),
                         IsActive = e.IsActive ? "Да" : "Не",
                     })
                .ToListAsync();

            if (size.HasValue && page.HasValue)
            {
                exams = exams
                    .OrderBy(e => e.Title)
                    .Skip((int)(page * size - size))
                    .Take((int)size).ToList();
            }

            var model = new ExamListVM()
            {
                PageNo = page,
                PageSize = size
            };

            model.TotalRecords = await this.repository.All<Exam>().Where(e => !e.IsDeleted).CountAsync();
            model.Exams = exams;

            return model;
        }

        public async Task<List<ViewExamVM>> GetExamsForUserAsync()
        {
            var exams = await this.repository.All<Exam>()
                .Where(e => !e.IsDeleted && e.IsActive)
                .Join(this.repository.All<Subject>(),
                      e => e.SubjectId,
                      s => s.Id,
                      (e, s) => new ViewExamVM()
                      {
                          Id = e.Id.ToString(),
                          Title = e.Title,
                          SubjectName = s.Name,
                          Description = e.Description
                      })
                .ToListAsync();

            return exams;
        }

        public async Task<EditExamVM> GetExamForEditAsync(string id)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(id.ToGuid());

            if (exam != null)
            {
                var subject = await this.repository.GetByIdAsync<Subject>(exam.SubjectId);

                if (subject != null)
                {
                    return new EditExamVM()
                    {
                        Id = exam.Id.ToString(),
                        Title = exam.Title,
                        Description = exam.Description,
                        MaxScore = exam.MaxScore,
                        SubjectName = subject.Name,
                    };
                }
                return new EditExamVM();
            }

            return new EditExamVM();
        }

        public async Task<ViewExamVM> GetExamForViewAsync(string id)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(id.ToGuid());

            if (exam != null)
            {
                var subject = await this.repository.GetByIdAsync<Subject>(exam.SubjectId);
                var hasQuestions = await this.repository.All<Question>().AnyAsync(q => q.ExamId == id.ToGuid() && !q.IsDeleted);

                if (hasQuestions)
                {
                    var test = id.ToGuid();

                    var questions = this.repository.All<Question>().Where(q => q.ExamId == test && !q.IsDeleted)
                        .OrderBy(q => q.CreateDate)
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
                                .ToList()
                        })
                        .ToList();

                    return new ViewExamVM
                    {
                        Id = exam.Id.ToString(),
                        Title = exam.Title,
                        Description = exam.Description,
                        SubjectName = subject.Name,
                        Questions = questions,
                    };
                }
                else
                {
                    return new ViewExamVM
                    {
                        Id = exam.Id.ToString(),
                        Title = exam.Title,
                        Description = exam.Description,
                        SubjectName = subject.Name,
                        Questions = new List<QuestionExamVM>(),
                    };
                }
            }

            return new ViewExamVM();
        }

        public async Task<ExamVM> GetExamInfoAsync(string id)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(id.ToGuid());

            if (exam != null)
            {
                var subject = await this.repository.GetByIdAsync<Subject>(exam.SubjectId);
                var questionsCount = await this.repository.AllReadonly<Question>()
                    .Where(q => q.ExamId == exam.Id && !q.IsDeleted)
                    .CountAsync();

                ExamVM model = new ExamVM
                {
                    Id = exam.Id.ToString(),
                    Title = exam.Title,
                    Description = exam.Description,
                    SubjectName = subject.Name,
                    QuestionsCount = questionsCount,
                };

                return model;
            }

            return new ExamVM();
        }

        public async Task<bool> HasAnyQuestionsAsync(string id)
        {
            bool hasAnyQuestions = await this.repository.All<Question>().AnyAsync(q => q.ExamId == id.ToGuid());

            return hasAnyQuestions;
        }

        public async Task<bool> QuestionsPointsSumEqualsMaxScoreAsync(string id)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(id.ToGuid());

            if (exam != null)
            {
                var questionsPointsSum = this.repository.All<Question>().Where(q => !q.IsDeleted).Sum(q => q.Points);

                return exam.MaxScore == questionsPointsSum;
            }

            return false;
        }
    }
}
