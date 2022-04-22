using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Core.Services
{
    public class AnswerOptionService : IAnswerOptionService
    {
        private readonly IApplicationDbRepository repository;

        public AnswerOptionService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Create(QuestionAnswerOptionVM model)
        {
            var answerOption = new AnswerOption
            {
                Content = model.AnswerOption,
            };

            await this.repository.AddAsync(answerOption);
            await this.repository.SaveChangesAsync();

            return true;
        }
    }
}
