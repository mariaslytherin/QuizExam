using QuizExam.Core.Models.Exam;

namespace QuizExam.Core.Contracts
{
    public interface ITakeExamService
    {
        Task<Guid> CreateTake(string userId, string examId);

        Task<ViewExamVM> GetExamForView(string id);

        Task<bool> TakeExists(string userId, string examId);
    }
}
