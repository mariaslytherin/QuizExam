using QuizExam.Core.Models.AnswerOption;

namespace QuizExam.Core.Contracts
{
    public interface IAnswerOptionService
    {
        Task<bool> CreateAsync(NewAnswerOptionVM model);

        Task<bool> HasLessThenSixOptionsAsync(string questionId);

        Task<IEnumerable<AnswerOptionVM>> GetOptionsAsync(string questionId);

        Task<bool> DeleteAsync(string id);

        Task<bool> SetCorrectAnswerAsync(SetCorrectAnswerVM model);
    }
}
