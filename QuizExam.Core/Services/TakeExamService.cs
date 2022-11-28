using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.Question;
using QuizExam.Core.Models.TakeExam;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Enums;
using QuizExam.Infrastructure.Data.Identity;
using QuizExam.Infrastructure.Data.Repositories;
using System.ComponentModel.DataAnnotations;

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

        public async Task<TakenExamsListVM> TakenExams(string id)
        {
            return new TakenExamsListVM();
        }

        public async Task<TakeExamVM> GetExamForView(string id)
        {
            try
            {
                var take = await this.repository.GetByIdAsync<TakeExam>(Guid.Parse(id));
                var exam = await this.repository.GetByIdAsync<Exam>(take.ExamId);
                var subject = await this.repository.GetByIdAsync<Subject>(exam.SubjectId);

                if (take != null)
                {
                    take.Status = TakeExamStatusEnum.Finished;
                    await this.repository.SaveChangesAsync();

                    var questions = await this.repository.All<Question>().Where(q => q.ExamId == exam.Id && !q.IsDeleted)
                        .Select(q => new QuestionExamVM
                        {
                            Content = q.Content,
                            Points = q.Points,
                            Rule = q.Rule,
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
    }
}
