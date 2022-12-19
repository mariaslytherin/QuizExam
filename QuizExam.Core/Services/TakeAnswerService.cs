using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.TakeAnswer;
using QuizExam.Core.Models.TakeExam;
using QuizExam.Core.Models.TakeQuestion;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Enums;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Core.Services
{
    public class TakeAnswerService : ITakeAnswerService
    {
        private readonly IApplicationDbRepository repository;

        public TakeAnswerService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> AddAnswer(TakeQuestionVM model, string examId)
        {
            try
            {
                var result = false;
                var answer = this.repository.All<TakeAnswer>()
                    .Where(t => t.QuestionId == Guid.Parse(model.QuestionId) &&
                                t.TakeExamId == Guid.Parse(model.TakeExamId))
                    .FirstOrDefault();

                if (answer != null)
                {
                    if (answer.AnswerOptionId != model.CheckedOptionId.ToGuid())
                    {
                        await DeleteAnswer(answer.Id);

                        var takeExam = await this.repository.GetByIdAsync<TakeExam>(model.TakeExamId.ToGuid());

                        if (takeExam != null)
                        {
                            var newAnswer = new TakeAnswer
                            {
                                TakeExamId = model.TakeExamId.ToGuid(),
                                AnswerOptionId = model.CheckedOptionId.ToGuid(),
                                QuestionId = model.QuestionId.ToGuid(),
                            };
                            await this.repository.AddAsync(newAnswer);
                            await this.repository.SaveChangesAsync();
                            result = true;
                        }

                        return result;
                    }
                    else
                    {
                        result = true;
                        return result;
                    }
                }
                else
                {
                    var takeExam = await this.repository.GetByIdAsync<TakeExam>(model.TakeExamId.ToGuid());

                    if (takeExam != null)
                    {
                        var newAnswer = new TakeAnswer
                        {
                            TakeExamId = model.TakeExamId.ToGuid(),
                            AnswerOptionId = model.CheckedOptionId.ToGuid(),
                            QuestionId = model.QuestionId.ToGuid(),
                        };
                        await this.repository.AddAsync(newAnswer);
                        await this.repository.SaveChangesAsync();
                        result = true;
                    }

                    return result;
                }
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(TakeAnswer)}'. ");
            }
        }

        private async Task DeleteAnswer(Guid answerId)
        {
            try
            {
                await this.repository.DeleteAsync<TakeAnswer>(answerId);
                await this.repository.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException($"Object of type '{nameof(TakeAnswer)}' was not found. ");
            }
        }
    }
}
