using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.AnswerOption;
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

        public async Task<Guid> Create(NewQuestionVM model)
        {
            try
            {
                bool isExam = await this.repository.GetByIdAsync<Exam>(model.ExamId.ToGuid()) != null ? true : false;

                if (isExam)
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
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Question)}'. ");
            }
        }

        public async Task<bool> Delete(string id)
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

        public async Task<bool> Edit(EditQuestionVM model)
        {
            try
            {
                bool result = false;
                var question = await this.repository.GetByIdAsync<Question>(model.Id.ToGuid());

                if (question != null)
                {
                    question.Content = model.Content;
                    question.Rule = model.Rule;
                    question.Points = model.Points;
                    question.ModifyDate = DateTime.Today;
                    await this.repository.SaveChangesAsync();
                    result = true;
                }

                return result;
            }
            catch
            {
                throw new NullReferenceException($"Object of type '{nameof(Question)}' was not found. ");
            }
        }

        public async Task<Question> GetQuestionById(string id)
        {
            var question = await this.repository.GetByIdAsync<Question>(id.ToGuid());

            return question;
        }

        public async Task<EditQuestionVM> GetQuestionForEdit(string id)
        {
            var question = await this.repository.GetByIdAsync<Question>(id.ToGuid());

            try
            {
                if (question != null)
                {
                    var model = new EditQuestionVM
                    {
                        Id = question.Id.ToString(),
                        Content = question.Content,
                        Rule = question.Rule,
                        Points = question.Points,
                    };

                    return model;
                }
                else
                {
                    return new EditQuestionVM();
                }
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException($"Object of type '{nameof(Question)}' was not found. ");
            }
        }

        public async Task<TakeQuestionVM> GetNextQuestion(string examId, string takeId, int order)
        {
            try
            {
                var allQuestions = await this.repository.All<Question>()
                    .Where(q => q.ExamId == examId.ToGuid() && !q.IsDeleted)
                    .Select(q => new TakeQuestionVM
                    {
                        QuestionId = q.Id.ToString(),
                        TakeExamId = takeId,
                        ExamId = q.ExamId.ToString(),
                        Content = q.Content,
                        Order = order,
                        TakeAnswers = this.repository.All<AnswerOption>().Where(t => t.QuestionId == q.Id && !t.IsDeleted)
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
                                        }).ToList(),
                    })
                    .ToListAsync();

                var question = allQuestions[order];

                if (allQuestions.Count() == order + 1)
                {
                    question.IsLast = true;
                    return question;
                }

                return question;
            }
            catch
            {
                throw new InvalidOperationException($"Object of type '{nameof(Question)}' was not found on index {order}. ");
            }
        }

        public async Task<TakeQuestionVM> GetPreviousQuestion(string examId, string takeId, int order)
        {
            try
            {
                var questionAnswers = await this.repository.All<Question>().Where(q => q.ExamId == examId.ToGuid() && !q.IsDeleted)
                            .Select(q => new TakeQuestionVM
                            {
                                QuestionId = q.Id.ToString(),
                                TakeExamId = takeId,
                                ExamId = q.ExamId.ToString(),
                                Content = q.Content,
                                Order = order,
                                TakeAnswers = this.repository.All<AnswerOption>().Where(t => t.QuestionId == q.Id && !t.IsDeleted)
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
                                        }).ToList(),
                            }).ToListAsync();

                var question = questionAnswers[order];

                return question;
            }
            catch
            {
                throw new InvalidOperationException($"Object of type '{nameof(Question)}' was not found on index {order}. ");
            }
        }

        public int GetLastNotTakenQuestionOrder(string takeId, string examId)
        {
            try
            {
                var takenQuestionsCount = this.repository.All<TakeAnswer>().Where(t => t.TakeExamId == takeId.ToGuid()).Count();
                var allQuestionsCount = this.repository.All<Question>().Where(q => q.ExamId == examId.ToGuid() && !q.IsDeleted).Count();
                var order = takenQuestionsCount;

                return order;
            }
            catch
            {

                throw;
            }
        }

        public async Task<Guid[]> GetQuestionIds(string examId)
        {
            var questionIds = await this.repository.All<Question>()
                .Where(q => q.ExamId == examId.ToGuid() && !q.IsDeleted)
                .Select(q => q.Id)
                .ToArrayAsync();

            return questionIds;
        }

        public bool HasAnswerOptions(string id)
        {
            var hasAnswerOptions = this.repository.All<AnswerOption>()
                .Any(a => a.QuestionId == id.ToGuid());

            return hasAnswerOptions;
        }
    }
}
