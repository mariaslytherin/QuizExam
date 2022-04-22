using QuizExam.Core.Models.Question;

namespace QuizExam.Core.Contracts
{
    public interface IQuestionService
    {
        Task<bool> Create(NewQuestionVM model);
    }
}
