using QuizExam.Core.Models.Question;
using QuizExam.Core.Models.TakeQuestion;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface IQuestionService
    {
        Task<Guid> Create(NewQuestionVM model);

        Task<Question> GetQuestionById(string id);

        Task<TakeQuestionVM> GetNextQuestion(string examId, string takeId, int order);

        Task<TakeQuestionVM> GetPreviousQuestion(string examId, string takeId, int order);

        Task<Guid[]> GetQuestionIds(string examId);

        bool HasAnswerOptions(string id);

        Task<EditQuestionVM?> GetQuestionForEdit(string id);

        Task<bool> Edit(EditQuestionVM model);

        Task<bool> Delete(string id);
    }
}
