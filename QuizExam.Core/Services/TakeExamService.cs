using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.Question;
using QuizExam.Core.Models.TakeExam;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Enums;
using QuizExam.Infrastructure.Data.Identity;
using QuizExam.Infrastructure.Data.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace QuizExam.Core.Services
{
    public class TakeExamService : ITakeExamService
    {
        private readonly IApplicationDbRepository repository;

        public TakeExamService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Guid> CreateTake(string userId, string examId)
        {
            try
            {
                var isUser = await this.repository.GetByIdAsync<ApplicationUser>(userId);
                var isExam = await this.repository.GetByIdAsync<Exam>(Guid.Parse(examId));

                if (isUser != null && isExam != null)
                {
                    TakeExam newTake = new TakeExam()
                    {
                        UserId = userId,
                        ExamId = Guid.Parse(examId),
                        Status = TakeExamStatusEnum.Started
                    };

                    await this.repository.AddAsync(newTake);
                    await this.repository.SaveChangesAsync();

                    return newTake.Id;
                }

                return Guid.Empty;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(TakeExam)}'. ");
            }
        }

        public async Task<TakeExam> GetTakeExamById(string takeId)
        {
            try
            {
                return await this.repository.GetByIdAsync<TakeExam>(takeId.ToGuid());
            }
            catch
            {
                throw new NullReferenceException($"Object of type '{nameof(Exam)}' was not found. ");
            }
        }

        public async Task<TakenExamsListVM> TakenExams(string id, int? page, int? size)
        {
            var takes = await this.repository.All<TakeExam>()
                .Where(t => t.UserId == id && t.Status == TakeExamStatusEnum.Finished)
                .Join(this.repository.All<Exam>(),
                      t => t.ExamId,
                      e => e.Id,
                     (t, e) => new TakeExamVM()
                     {
                         Id = t.Id.ToString(),
                         Title = e.Title,
                         SubjectName = this.repository.All<Subject>()
                                        .Where(s => s.Id == e.SubjectId)
                                        .Select(s => s.Name)
                                        .FirstOrDefault(),
                         CreateDate = t.CreateDate.ToDateOnlyString(),
                         ResultScore = t.Score,
                         MaxScore = e.MaxScore,
                     })
            .ToListAsync();

            if (size.HasValue && page.HasValue)
            {
                takes = takes
                    .OrderBy(e => e.Title)
                    .Skip((int)(page * size - size))
                    .Take((int)size).ToList();
            }

            var model = new TakenExamsListVM()
            {
                PageNo = page,
                PageSize = size
            };

            model.TotalRecords = await this.repository.All<Exam>().Where(e => !e.IsDeleted).CountAsync();
            model.TakenExams = takes;

            return model;
        }

        public async Task<UncompletedExamsVM> UncompletedExams(string id, int? page, int? size)
        {
            var exams = await this.repository.All<Exam>()
                .GroupJoin(this.repository.All<TakeExam>().Where(t => t.UserId == id && t.Status != TakeExamStatusEnum.Finished),
                    exam => exam.Id,
                    take => take.ExamId,
                    (exam, take) => new
                    {
                        exam,
                        take,
                    })
                .SelectMany(
                    x => x.take.Where(t => t != null),
                    (exam, take) => new UncompletedExamVM
                    {
                        TakeId = take.Id.ToString(),
                        Title = exam.exam.Title,
                        AllQuestionsCount = exam.exam.Questions.Count(q => !q.IsDeleted),
                        TakenQuestionsCount = take.TakeAnswers.Count(t => !t.IsDeleted),
                        SubjectName = exam.exam.Subject.Name,
                        StartDate = take.CreateDate.ToDateOnlyString(),
                    }).ToListAsync();

            var model = new UncompletedExamsVM()
            {
                PageNo = page,
                PageSize = size
            };

            model.TotalRecords = exams.Count();

            if(size.HasValue && page.HasValue)
            {
                exams = exams
                    .OrderBy(e => e.Title)
                    .Skip((int)(page * size - size))
                    .Take((int)size).ToList();
            }

            model.UncompletedExams = exams;

            return model;
        }

        public async Task<TakeExamVM> GetExamForView(string takeExamId)
        {
            try
            {
                var take = await this.repository.GetByIdAsync<TakeExam>(Guid.Parse(takeExamId));
                var exam = await this.repository.GetByIdAsync<Exam>(take.ExamId);
                var subject = await this.repository.GetByIdAsync<Subject>(exam.SubjectId);

                if (take != null)
                {
                    var questions = await this.repository.All<Question>().Where(q => q.ExamId == exam.Id && !q.IsDeleted)
                        .OrderBy(q => q.CreateDate)
                        .Select(q => new QuestionExamVM
                        {
                            Content = q.Content,
                            Points = q.Points,
                            Rule = q.Rule,
                            AnswerOptions = this.repository.All<AnswerOption>().Where(t => t.QuestionId == q.Id && !t.IsDeleted)
                                .GroupJoin(this.repository.All<TakeAnswer>().Where(t => t.TakeExamId == take.Id),
                                    option => option.Id,
                                    answer => answer.AnswerOptionId,
                                    (option, answer) => new
                                    {
                                        option,
                                        answer,
                                    })
                                .SelectMany(
                                    x => x.answer.DefaultIfEmpty(),
                                    (option, answer) => new AnswerOptionVM
                                    {
                                        Id = answer.Id.ToString(),
                                        Content = option.option.Content,
                                        IsCorrect = option.option.IsCorrect,
                                    }).ToList(),
                        }).ToListAsync();

                    var resultScore = questions.Where(q => q.AnswerOptions.Any(a => a.IsCorrect && a.Id is not null)).Select(q => q.Points).Sum();

                    return new TakeExamVM
                    {
                        Id = take.Id.ToString(),
                        Title = exam.Title,
                        SubjectName = subject.Name,
                        MaxScore = exam.MaxScore,
                        ResultScore = resultScore,
                        Questions = questions,
                    };
                }
                else
                {
                    return new TakeExamVM
                    {
                        Id = exam.Id.ToString(),
                        Title = exam.Title,
                        SubjectName = subject.Name,
                        MaxScore = exam.MaxScore,
                        Questions = new List<QuestionExamVM>(),
                    };
                }
            }
            catch
            {
                throw new ArgumentNullException($"Object of type '{nameof(Exam)}' was not found. ");
            }
        }

        public async Task<bool> TakeExists(string userId, string examId)
        {
            try
            {
                var takeExists = await this.repository.All<TakeExam>()
                    .AnyAsync(t => t.ExamId.ToString() == examId && t.UserId == userId && (t.Status != TakeExamStatusEnum.Finished));

                return takeExists;
            }
            catch
            {
                throw new Exception($"An error appeard.");
            }
        }

        public async Task<bool> FinishExam(string takeExamId)
        {
            var take = await this.repository.GetByIdAsync<TakeExam>(Guid.Parse(takeExamId));
            bool result = false;

            if (take != null)
            {
                var questionAnswers = await this.repository.All<Question>().Where(q => q.ExamId == take.ExamId && !q.IsDeleted)
                            .Select(q => new
                            {
                                Points = q.Points,
                                AnswerOptions = this.repository.All<AnswerOption>().Where(t => t.QuestionId == q.Id)
                                    .GroupJoin(this.repository.All<TakeAnswer>().Where(t => t.TakeExamId == take.Id),
                                        option => option.Id,
                                        answer => answer.AnswerOptionId,
                                        (option, answer) => new
                                        {
                                            option,
                                            answer,
                                        })
                                    .SelectMany(
                                        x => x.answer.DefaultIfEmpty(),
                                        (option, answer) => new
                                        {
                                            Id = answer.Id.ToString(),
                                            IsCorrect = option.option.IsCorrect,
                                        }).ToList(),
                            }).ToListAsync();

                var resultScore = questionAnswers.Where(q => q.AnswerOptions.Any(a => a.IsCorrect && a.Id is not null)).Select(q => q.Points).Sum();

                take.Score = resultScore;
                take.Status = TakeExamStatusEnum.Finished;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }
    }
}
