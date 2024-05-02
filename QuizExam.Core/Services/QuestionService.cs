using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.Question;
using QuizExam.Core.Models.TakeAnswer;
using QuizExam.Core.Models.TakeQuestion;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Enums;
using QuizExam.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizExam.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IApplicationDbRepository repository;

        public QuestionService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Guid> CreateAsync(NewQuestionVM model)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(model.ExamId.ToGuid());

            if (exam != null)
            {
                var question = new Question
                {
                    ExamId = model.ExamId.ToGuid(),
                    Content = model.Content,
                    Rule = model.Rule,
                    Points = model.Points,
                };

                await this.repository.AddAsync(question);
                await this.repository.SaveChangesAsync();

                return question.Id;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            bool result = false;
            var question = await this.repository.GetByIdAsync<Question>(id.ToGuid());

            if (question != null)
            {
                question.IsDeleted = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> EditAsync(EditQuestionVM model)
        {
            bool result = false;
            var question = await this.repository.GetByIdAsync<Question>(model.Id.ToGuid());

            if (question != null)
            {
                question.Content = model.QuestionContent;
                question.Rule = model.Rule;
                question.Points = model.Points;
                question.ModifyDate = DateTime.Today;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<Question> GetQuestionByIdAsync(string id)
        {
            var question = await this.repository.GetByIdAsync<Question>(id.ToGuid());

            return question;
        }

        public async Task<EditQuestionVM> GetQuestionForEditAsync(string id)
        {
            var question = await this.repository.GetByIdAsync<Question>(id.ToGuid());

            if (question != null)
            {
                var model = new EditQuestionVM
                {
                    Id = question.Id.ToString(),
                    QuestionContent = question.Content,
                    Rule = question.Rule,
                    Points = question.Points,
                };

                return model;
            }

            return new EditQuestionVM();
        }

        public async Task<TakeQuestionVM> GetNextQuestionAsync(string examId, string takeId, int order)
        {
            var allQuestions = await this.GetQuestionWithAnswers(examId, takeId, order);

            if (allQuestions.Count() != 0)
            {
                var question = allQuestions[order];
                var takeExam = await this.repository.GetByIdAsync<TakeExam>(takeId.ToGuid());
                question.Mode = takeExam.Mode;

                if (takeExam.Mode == TakeExamModeEnum.Exercise)
                {
                    question.TimePassed = takeExam.TimePassed.ToString();
                }
                else
                {
                    var exam = await this.repository.GetByIdAsync<Exam>(examId.ToGuid());
                    question.Duration = exam.Duration.ToString();
                }

                if (allQuestions.Count() == order + 1)
                {
                    question.IsLast = true;
                    return question;
                }

                return question;
            }

            return new TakeQuestionVM();
        }

        public async Task<TakeQuestionVM> GetPreviousQuestionAsync(string examId, string takeId, int order)
        {
            var allQuestions = await this.GetQuestionWithAnswers(examId, takeId, order);

            if (allQuestions.Count() != 0)
            {
                var question = allQuestions[order];

                return question;
            }

            return new TakeQuestionVM();
        }

        private async Task<List<TakeQuestionVM>> GetQuestionWithAnswers(string examId, string takeId, int order)
        {
            return await this.repository.All<Question>()
                .Where(q => q.ExamId == examId.ToGuid() && !q.IsDeleted)
                .OrderBy(q => q.CreateDate)
                .Select(q => new TakeQuestionVM
                {
                    QuestionId = q.Id.ToString(),
                    TakeExamId = takeId,
                    ExamId = q.ExamId.ToString(),
                    Content = q.Content,
                    Order = order,
                    TakeAnswers = this.repository.All<AnswerOption>().Where(t => t.QuestionId == q.Id && !t.IsDeleted)
                            .OrderBy(t => t.CreateDate)
                            .GroupJoin(this.repository.All<TakeAnswer>().Where(t => t.TakeExamId == takeId.ToGuid() && !t.IsDeleted),
                                option => option.Id,
                                answer => answer.AnswerOptionId,
                                (option, answer) => new
                                {
                                    option,
                                    answer,
                                })
                            .SelectMany(
                                x => x.answer.DefaultIfEmpty(),
                                (option, answer) => new TakeAnswerVM
                                {
                                    AnswerId = answer.Id.ToString(),
                                    OptionId = option.option.Id.ToString(),
                                    IsChecked = answer.Id.ToString() != null ? true : false,
                                    Content = option.option.Content,
                                })
                            .ToList(),
                }).ToListAsync();
        }

        public int GetLastNotTakenQuestionOrder(string takeId)
        {
            var takenQuestionsCount = this.repository.All<TakeAnswer>().Where(t => t.TakeExamId == takeId.ToGuid()).Count();

            return takenQuestionsCount;
        }

        public async Task<bool> HasEnoughAnswerOptionsAsync(string id)
        {
            var hasEnoughAnswerOptions = await this.repository.All<AnswerOption>()
                .CountAsync(a => a.QuestionId == id.ToGuid() && !a.IsDeleted) >= 2;

            return hasEnoughAnswerOptions;
        }
    }
}
