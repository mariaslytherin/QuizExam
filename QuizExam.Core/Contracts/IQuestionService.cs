using QuizExam.Core.Models.Question;
using QuizExam.Core.Models.TakeQuestion;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface IQuestionService
    {
        Task<Guid> CreateAsync(NewQuestionVM model);

        Task<Question> GetQuestionByIdAsync(string id);

        Task<TakeQuestionVM> GetNextQuestionAsync(string examId, string takeId, int order);

        Task<TakeQuestionVM> GetPreviousQuestionAsync(string examId, string takeId, int order);

        int GetLastNotTakenQuestionOrder(string takeId);

        Task<bool> HasEnoughAnswerOptionsAsync(string id);

        Task<EditQuestionVM> GetQuestionForEditAsync(string id);

        Task<bool> EditAsync(EditQuestionVM model);

        Task<bool> DeleteAsync(string id);
    }
}
