using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
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

        public async Task<bool> CreateAsync(NewAnswerOptionVM model)
        {
            var question = await this.repository.GetByIdAsync<Question>(model.QuestionId.ToGuid());

            if (question != null)
            {
                var answerOption = new AnswerOption
                {
                    QuestionId = question.Id,
                    Content = model.OptionContent,
                };

                await this.repository.AddAsync(answerOption);
                await this.repository.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> HasLessThenSixOptionsAsync(string questionId)
        {
            var optionsCount = await this.repository.AllReadonly<AnswerOption>().Where(a => a.QuestionId == questionId.ToGuid()).CountAsync();
            return optionsCount <= 6;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            bool result = false;
            var option = await this.repository.GetByIdAsync<AnswerOption>(id.ToGuid());

            if (option != null)
            {
                option.IsDeleted = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<IEnumerable<AnswerOptionVM>> GetOptionsAsync(string questionId)
        {
            var question = await this.repository.GetByIdAsync<Question>(questionId.ToGuid());

            if (question != null)
            {
                var answerOptions = await this.repository.All<AnswerOption>()
                    .Where(a => a.QuestionId == questionId.ToGuid() && !a.IsDeleted)
                    .OrderBy(x => x.CreateDate)
                    .Select(a => new AnswerOptionVM
                    {
                        Id = a.Id.ToString(),
                        Content = a.Content,
                        IsCorrect = a.IsCorrect,
                    })
                    .ToListAsync();

                return answerOptions;
            }

            return Enumerable.Empty<AnswerOptionVM>();
        }

        public async Task<bool> SetCorrectAnswerAsync(SetCorrectAnswerVM model)
        {
            try
            {
                bool result = false;

                foreach (var option in model.Options)
                {
                    var currentOption = await this.repository.GetByIdAsync<AnswerOption>(option.Id.ToGuid());
                    currentOption.IsCorrect = false;
                    await this.repository.SaveChangesAsync();
                }

                var correctAnswer = await this.repository.GetByIdAsync<AnswerOption>(model.CorrectAnswerId.ToGuid());

                if (correctAnswer != null)
                {
                    correctAnswer.IsCorrect = true;
                    await this.repository.SaveChangesAsync();
                    result = true;
                }

                return result;
            }
            catch
            {
                throw new NullReferenceException($"Obejct of type '{nameof(AnswerOption)}' was not found. ");
            }
        }
    }
}
