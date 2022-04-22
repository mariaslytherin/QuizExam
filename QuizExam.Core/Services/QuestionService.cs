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

        public async Task<bool> Create(NewQuestionVM model)
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

            return true;
        }
    }
}
