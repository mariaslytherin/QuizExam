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

        public async Task<bool> Create(AddAnswerOptionVM model)
        {
            var answerOption = new AnswerOption
            {
                QuestionId = Guid.Parse(model.QuestionId),
                Content = model.AnswerOption,
            };

            await this.repository.AddAsync(answerOption);
            await this.repository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<AnswerOption> GetOptions(string questionId)
        {
            var answerOptions = this.repository.All<AnswerOption>()
                .Where(a => a.QuestionId == Guid.Parse(questionId))
                .DefaultIfEmpty()
                .ToList();

            return answerOptions;
        }
    }
}
