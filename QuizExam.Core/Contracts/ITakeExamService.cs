using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.TakeExam;

namespace QuizExam.Core.Contracts
{
    public interface ITakeExamService
    {
        Task<Guid> CreateTake(string userId, string examId);

        Task<TakeExamVM> GetExamForView(string id);

        Task<bool> TakeExists(string userId, string examId);

        Task<TakenExamsListVM> TakenExams(string userId);
    }
}
