using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Question;
using QuizExam.Infrastructure.Data;
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
                bool isExam = await this.repository.GetByIdAsync<Exam>(Guid.Parse(model.ExamId)) != null ? true : false;

                if (isExam)
                {
                    var question = new Question
                    {
                        ExamId = Guid.Parse(model.ExamId),
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
            var question = await this.repository.GetByIdAsync<Question>(Guid.Parse(id));

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
            bool result = false;
            var question = await this.repository.GetByIdAsync<Question>(Guid.Parse(model.Id));

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

        public async Task<Question> GetQuestionById(string id)
        {
            var question = await this.repository.GetByIdAsync<Question>(Guid.Parse(id));

            return question;
        }

        public async Task<EditQuestionVM> GetQuestionForEdit(string id)
        {
            var question = await this.repository.GetByIdAsync<Question>(Guid.Parse(id));
            var model = new EditQuestionVM
            {
                Id = question.Id.ToString(),
                Content = question.Content,
                Rule = question.Rule,
                Points = question.Points,
            };

            return model;
        }

        public async Task<Question> GetQuestionForTake(string examId, int order)
        {
            var allQuestions = await this.repository.All<Question>()
                .Where(q => q.ExamId == Guid.Parse(examId) && !q.IsDeleted)
                .ToListAsync();

            return allQuestions[order];
        }

        public async Task<Guid[]> GetQuestionIds(string examId)
        {
            var questionIds = await this.repository.All<Question>()
                .Where(q => q.ExamId == Guid.Parse(examId) && !q.IsDeleted)
                .Select(q => q.Id)
                .ToArrayAsync();

            return questionIds;
        }

        public bool HasAnswerOptions(string id)
        {
            var hasAnswerOptions = this.repository.All<AnswerOption>()
                .Any(a => a.QuestionId == Guid.Parse(id));

            return hasAnswerOptions;
        }
    }
}
