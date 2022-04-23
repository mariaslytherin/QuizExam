using QuizExam.Core.Contracts;
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

        public async Task<Question> GetQuestionById(string id)
        {
            var question = await this.repository.GetByIdAsync<Question>(Guid.Parse(id));

            return question;
        }

        public bool HasAnswerOptions(string id)
        {
            var hasAnswerOptions = this.repository.All<AnswerOption>()
                .Any(a => a.QuestionId == Guid.Parse(id));

            return hasAnswerOptions;
        }
    }
}
