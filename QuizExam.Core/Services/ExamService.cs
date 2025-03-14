using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.Question;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Enums;
using QuizExam.Infrastructure.Data.Identity;
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
                exam.ModifyDate = DateTime.Now;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task CreateAsync(string userId, NewExamVM model)
        {
            var exam = new Exam()
            {
                Title = model.Title,
                Description = model.Description,
                MaxScore = model.MaxScore,
                SubjectId = model.SubjectId.ToGuid(),
                Duration = TimeSpan.Parse(model.Duration),
                UserId = userId,
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
                exam.ModifyDate = DateTime.Now;
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
                exam.Duration = TimeSpan.Parse(model.Duration);
                exam.ModifyDate = DateTime.Today;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<List<Exam>> GetExamsByUserId(string userId)
        {
            return await this.repository.All<Exam>().Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<ExamListVM> GetAllExamsAsync(string userId, bool isSuperAdmin, int? page, int? size)
        {
            List<Exam> allExams = await this.repository.All<Exam>().OrderByDescending(e => e.CreateDate).ToListAsync();

            if (!isSuperAdmin)
            {
                allExams = allExams.Where(e => e.UserId == userId).ToList();
            }

            var exams = allExams
                .Where(e => !e.IsDeleted)
                .Join(this.repository.All<Subject>(),
                      e => e.SubjectId,
                      s => s.Id,
                     (e, s) => new { e, s })
                .Join(this.repository.All<ApplicationUser>(),
                      es => es.e.UserId,
                      u => u.Id,
                     (es, u ) => new ViewExamVM()
                     {
                         Id = es.e.Id.ToString(),
                         Title = es.e.Title,
                         SubjectName = es.s.Name,
                         Description = es.e.Description,
                         CreatedBy = u.FirstName + " " + u.LastName,
                         CreateDate = es.e.CreateDate.ToDateOnlyString(),
                         IsActive = es.e.IsActive ? "Да" : "Не",
                     })
                .ToList();

            var model = new ExamListVM()
            {
                PageNo = page,
                PageSize = size
            };

            model.TotalRecords = exams.Count();
            if (size.HasValue && page.HasValue)
            {
                exams = exams
                    .Skip((int)(page * size - size))
                    .Take((int)size).ToList();
            }

            model.Exams = exams;

            return model;
        }

        public async Task<List<ViewExamVM>> GetActiveExamsAsync(string subjectId = null, string examTitle = null)
        {
            var exams = await this.repository.AllReadonly<Exam>()
                .Where(e => !e.IsDeleted && e.IsActive)
                .OrderByDescending(e => e.CreateDate)
                .ToListAsync();

            if (!String.IsNullOrEmpty(subjectId))
            {
                exams = exams.Where(e => e.SubjectId == subjectId.ToGuid()).ToList();
            }

            if (!String.IsNullOrEmpty(examTitle))
            {
                exams = exams.Where(e => e.Title.ToLower().Contains(examTitle.ToLower())).ToList();
            }

            var result = exams
                .Join(this.repository.AllReadonly<Subject>(),
                      e => e.SubjectId,
                      s => s.Id,
                      (e, s) => new ViewExamVM()
                      {
                          Id = e.Id.ToString(),
                          Title = e.Title,
                          SubjectName = s.Name,
                          Description = !String.IsNullOrEmpty(e.Description) && e.Description.Length > 200 ? e.Description.Substring(0, 140) + "..." : e.Description
                      })
                .ToList();

            return result;
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
                        Duration = exam.Duration.ToString(@"hh\:mm"),
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
                    var questions = this.repository.All<Question>().Where(q => q.ExamId == id.ToGuid() && !q.IsDeleted)
                        .OrderBy(q => q.CreateDate)
                        .Select(q => new QuestionExamVM
                        {
                            Id = q.Id.ToString(),
                            Content = q.Content,
                            Points = q.Points,
                            AnswerOptions = this.repository
                                .All<AnswerOption>()
                                .Where(a => a.QuestionId == q.Id && !a.IsDeleted)
                                .OrderBy(a => a.CreateDate)
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
                        IsActive = exam.IsActive.ToString(),
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
                        IsActive = exam.IsActive.ToString(),
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
                    Duration = exam.Duration.ToString(),
                };

                return model;
            }

            return new ExamVM();
        }

        public async Task<bool> IsExamDeactivatedAsync(string id)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(id.ToGuid());

            if (exam != null)
            {
                return !exam.IsActive;
            }

            return false;
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
                var questionsPointsSum = this.repository.All<Question>().Where(q => !q.IsDeleted && q.ExamId == exam.Id).Sum(q => q.Points);

                return exam.MaxScore == questionsPointsSum;
            }

            return false;
        }

        public async Task<bool> HasQuestionsWithoutSetCorrectAnswerAsync(string id)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(id.ToGuid());

            if (exam != null)
            {
                var hasQuestionsWithoutCorrectAnswer = this.repository.AllReadonly<Question>().Where(q => !q.IsDeleted && q.ExamId == exam.Id)
                    .Include(q => q.Answers)
                    .Any(q => !q.Answers.Where(o => !o.IsDeleted).Any(a => a.IsCorrect));

                return hasQuestionsWithoutCorrectAnswer;
            }

            return false;
        }

        public async Task<IEnumerable<Exam>> GetActiveExamsBySubjectAsync(string subjectId)
        {
            return await this.repository.AllReadonly<Exam>()
                .Where(e => e.SubjectId == subjectId.ToGuid() && e.IsActive)
                .ToListAsync();
        }

        public List<HardestQuestionInfoVM> GetExamTop5HardestQuestionsAsync(string examId)
        {
            if (examId.ToGuid() == Guid.Empty)
            {
                return new List<HardestQuestionInfoVM>();
            }

            var hardestQuestions = this.repository.AllReadonly<TakeAnswer>()
                .Include(ta => ta.TakeExam)
                .Include(ta => ta.AnswerOption)
                .ThenInclude(q => q.Question)
                .Where(ta => ta.TakeExam.ExamId == examId.ToGuid() && ta.TakeExam.Status == TakeExamStatusEnum.Finished && ta.AnswerOption.IsCorrect == false)
                .GroupBy(ta => new { ta.QuestionId, ta.Question.Content, ta.Question.Rule })
                .Select(g => new
                {
                    QuestionId = g.Key.QuestionId,
                    Content = g.Key.Content,
                    Rule = g.Key.Rule,
                    DifficultyRatio = g.Count() * 1.0 / this.repository.AllReadonly<TakeAnswer>().Count(ta => ta.QuestionId == g.Key.QuestionId)
                })
                .OrderByDescending(x => x.DifficultyRatio)
                .Take(5)
                .ToList();

            var result = new List<HardestQuestionInfoVM>();
            foreach (var question in hardestQuestions)
            {
                var usersTakenExam = this.repository.AllReadonly<TakeExam>()
                    .Include(te => te.TakeAnswers)
                    .ThenInclude(ta => ta.AnswerOption)
                    .Where(te => te.TakeAnswers.Any(ta => ta.QuestionId == question.QuestionId && ta.AnswerOption.IsCorrect == false) && te.Status == TakeExamStatusEnum.Finished)
                    .Select(te => te.UserId)
                    .Distinct()
                    .Count() * 100;
                
                var incorrectPercentage = usersTakenExam / this.repository.AllReadonly<TakeExam>()
                                            .Where(t => t.ExamId == examId.ToGuid() && t.Status == TakeExamStatusEnum.Finished)
                                            .Select(te => te.UserId).Distinct().Count();

                var options = this.repository.AllReadonly<AnswerOption>()
                                .Where(a => a.QuestionId == question.QuestionId && !a.IsDeleted)
                                .OrderBy(a => a.CreateDate)
                                .Select(a => new AnswerOptionVM
                                {
                                    Content = a.Content,
                                    IsCorrect = a.IsCorrect,
                                })
                                .ToList();

                result.Add(new HardestQuestionInfoVM
                {
                    QuestionId = question.QuestionId.ToString(),
                    Content = question.Content,
                    Rule = question.Rule,
                    MistakePercentage = incorrectPercentage,
                    AnswerOptions = options,
                });
            }

            return result.OrderByDescending(x => x.MistakePercentage).ToList();
        }
    }
}
