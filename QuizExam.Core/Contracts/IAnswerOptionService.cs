using QuizExam.Core.Models.AnswerOption;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface IAnswerOptionService
    {
        Task<bool> Create(AddAnswerOptionVM model);

        IEnumerable<AnswerOptionVM> GetOptions(string questionId);

        Task<bool> Delete(string id);

        Task<bool> SetCorrectAnswer(SetCorrectAnswerVM model);
    }
}
