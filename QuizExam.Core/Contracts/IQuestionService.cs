using QuizExam.Core.Models.Question;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface IQuestionService
    {
        Task<Guid> Create(NewQuestionVM model);

        Task<Question> GetQuestionById(string id);

        Task<Question> GetQuestionForTake(string examId, int order);

        Task<Guid[]> GetQuestionIds(string examId);

        bool HasAnswerOptions(string id);

        Task<EditQuestionVM> GetQuestionForEdit(string id);

        Task<bool> Edit(EditQuestionVM model);

        Task<bool> Delete(string id);
    }
}
