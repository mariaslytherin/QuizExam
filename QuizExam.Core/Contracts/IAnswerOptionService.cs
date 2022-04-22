using QuizExam.Core.Models.AnswerOption;

namespace QuizExam.Core.Contracts
{
    public interface IAnswerOptionService
    {
        Task<bool> Create(QuestionAnswerOptionVM model);
    }
}
