using QuizExam.Core.Models.Question;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface IQuestionService
    {
        Task<Guid> Create(NewQuestionVM model);

        Task<Question> GetQuestionById(string id);

        bool HasAnswerOptions(string id);
    }
}
