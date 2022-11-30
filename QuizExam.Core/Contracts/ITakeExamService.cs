using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.TakeExam;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface ITakeExamService
    {
        Task<Guid> CreateTake(string userId, string examId);

        Task<TakeExamVM> GetExamForView(string takeExamId);

        Task<bool> TakeExists(string userId, string examId);

        Task<TakenExamsListVM> TakenExams(string userId, int? page, int? size);

        Task<bool> FinishExam(string takeExamId);
    }
}
