using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.TakeExam;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Enums;

namespace QuizExam.Core.Contracts
{
    public interface ITakeExamService
    {
        Task<Guid> CreateTake(string userId, string examId, TakeExamModeEnum mode);

        Task<TakeExam> GetTakeExamById(string takeId);

        Task<TakeExamVM> GetTakeForView(string takeExamId, string? filter = null);

        Task<bool> TakeExists(string userId, string examId);

        Task<TakenExamsListVM> TakenExams(string userId, int? page, int? size);

        Task<UncompletedExamsVM> UncompletedExams(string userId, int? page, int? size);

        Task<bool> PuaseExam(string takeExamId, string timePassed);

        Task<bool> FinishExam(string takeExamId, string timePassed = null);
    }
}
