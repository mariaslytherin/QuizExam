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

        public async Task<bool> Delete(string id)
        {
            bool result = false;
            var option = await this.repository.GetByIdAsync<AnswerOption>(Guid.Parse(id));

            if (option != null)
            {
                option.IsDeleted = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public IEnumerable<AnswerOption> GetOptions(string questionId)
        {
            var answerOptions = this.repository.All<AnswerOption>()
                .Where(a => a.QuestionId == Guid.Parse(questionId) && !a.IsDeleted)
                .ToList();

            return answerOptions;
        }
    }
}
