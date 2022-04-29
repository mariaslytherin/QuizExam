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
            try
            {
                var question = await this.repository.GetByIdAsync<Question>(Guid.Parse(model.QuestionId));

                if (question != null)
                {
                    var answerOption = new AnswerOption
                    {
                        QuestionId = question.Id,
                        Content = model.AnswerOption,
                    };

                    await this.repository.AddAsync(answerOption);
                    await this.repository.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (NullReferenceException)
            {
                throw new InvalidOperationException($"Obejct of type '{nameof(Question)}' was not found. ");
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AnswerOption)}'. ");
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
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
            catch (NullReferenceException)
            {
                throw new InvalidOperationException($"Obejct of type '{nameof(AnswerOption)}' was not found. ");
            }
        }

        public IEnumerable<AnswerOptionVM> GetOptions(string questionId)
        {
            try
            {
                if (questionId != null)
                {
                    var answerOptions = this.repository.All<AnswerOption>()
                        .Where(a => a.QuestionId == Guid.Parse(questionId) && !a.IsDeleted)
                        .Select(a => new AnswerOptionVM
                        {
                            Id = a.Id.ToString(),
                            Content = a.Content,
                            IsCorrect = a.IsCorrect,
                        })
                        .ToList();

                    return answerOptions;
                }

                return Enumerable.Empty<AnswerOptionVM>();
            }
            catch
            {
                throw new InvalidOperationException($"Could not get sequence of type '{nameof(AnswerOption)}'. ");
            }
        }

        public async Task<bool> SetCorrectAnswer(SetCorrectAnswerVM model)
        {
            try
            {
                bool result = false;

                foreach (var option in model.Options)
                {
                    var currentOption = await this.repository.GetByIdAsync<AnswerOption>(Guid.Parse(option.Id));
                    currentOption.IsCorrect = false;
                    await this.repository.SaveChangesAsync();
                }

                var correctAnswer = await this.repository.GetByIdAsync<AnswerOption>(Guid.Parse(model.CorrectAnswerId));

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
                throw new InvalidOperationException($"Obejct of type '{nameof(AnswerOption)}' was not found. ");
            }
        }
    }
}
