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

        public async Task<Guid> CreateTake(string userId, string examId, TakeExamModeEnum mode)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(Guid.Parse(examId));

            if (exam != null)
            {
                TakeExam newTake = new TakeExam();

                if (mode == TakeExamModeEnum.Exercise)
                {
                    newTake = new TakeExam()
                    {
                        UserId = userId,
                        ExamId = examId.ToGuid(),
                        Status = TakeExamStatusEnum.Started,
                        TimePassed = new TimeSpan(0, 0, 0),
                        Mode = mode,
                        ModifyDate = DateTime.Now,
                    };
                }
                else
                {
                    newTake = new TakeExam()
                    {
                        UserId = userId,
                        ExamId = examId.ToGuid(),
                        Status = TakeExamStatusEnum.Started,
                        TimePassed = new TimeSpan(0, 0, 0),
                        Duration = exam.Duration,
                        Mode = mode,
                        ModifyDate = DateTime.Now,
                    };
                }
                
                await this.repository.AddAsync(newTake);
                await this.repository.SaveChangesAsync();

                return newTake.Id;
            }

            return Guid.Empty;
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
            var takes = await this.repository.AllReadonly<TakeExam>()
                .Where(t => t.UserId == id && t.Status == TakeExamStatusEnum.Finished)
                .OrderByDescending(t => t.CreateDate)
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
                         Mode = t.Mode,
                         CreateDate = t.CreateDate.ToDateOnlyString(),
                         ResultScore = t.Score,
                         MaxScore = e.MaxScore,
                     })
            .ToListAsync();

            var model = new TakenExamsListVM()
            {
                PageNo = page,
                PageSize = size
            };

            model.TotalRecords = takes.Count();
            if (size.HasValue && page.HasValue)
            {
                takes = takes
                    .OrderBy(e => e.CreateDate)
                    .Skip((int)(page * size - size))
                    .Take((int)size)
                    .ToList();
            }

            model.TakenExams = takes;

            return model;
        }

        public async Task<UncompletedExamsVM> UncompletedExams(string id, int? page, int? size)
        {
            // TODO  && t.Mode == TakeExamModeEnum.Exercise
            var exams = await this.repository.All<Exam>()
                .GroupJoin(this.repository.All<TakeExam>().Where(t => t.UserId == id && t.Status != TakeExamStatusEnum.Finished && t.Mode == TakeExamModeEnum.Exercise),
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

            if (size.HasValue && page.HasValue)
            {
                exams = exams
                    .OrderByDescending(e => e.StartDate)
                    .Skip((int)(page * size - size))
                    .Take((int)size).ToList();
            }

            model.UncompletedExams = exams;

            return model;
        }

        public async Task<TakeExamVM> GetTakeForView(string takeExamId, string? filter = null)
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
                            .OrderBy(t => t.CreateDate)
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

                if (filter is not null)
                {
                    if (filter == "correct")
                    {
                        questions = questions.Where(q => q.AnswerOptions.Any(a => a.IsCorrect && a.Id is not null)).ToList();
                    }
                    else if (filter == "incorrect")
                    {
                        questions = questions.Where(q => q.AnswerOptions.Any(a => (!a.IsCorrect && a.Id is not null) || (a.IsCorrect && a.Id is null))).ToList();
                    }
                }

                return new TakeExamVM
                {
                    Id = take.Id.ToString(),
                    Title = exam.Title,
                    SubjectName = subject.Name,
                    TimePassed = take.TimePassed.ToString(),
                    Duration = take.Duration.ToString(),
                    Mode = take.Mode,
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

        public async Task<bool> PuaseExam(string takeExamId, string timePassed)
        {
            var take = await this.repository.GetByIdAsync<TakeExam>(Guid.Parse(takeExamId));
            bool result = false;

            if (take != null)
            {
                take.TimePassed = TimeSpan.Parse(timePassed);
                take.Status = TakeExamStatusEnum.Paused;
                take.ModifyDate = DateTime.Now;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> FinishExam(string takeExamId, string timePassed = null)
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
                if (take.Mode == TakeExamModeEnum.Train)
                {
                    TimeSpan parsedTime = TimeSpan.Parse(timePassed);
                    if (parsedTime.Hours == 0 && parsedTime.Minutes == 0 && parsedTime.Seconds == 0)
                    {
                        take.TimePassed = take.Duration;
                    }
                    else
                    {
                        take.TimePassed = take.Duration - parsedTime;
                    }
                }
                else
                {
                    take.TimePassed = TimeSpan.Parse(timePassed);
                }
                take.ModifyDate = DateTime.Now;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }
    }
}
