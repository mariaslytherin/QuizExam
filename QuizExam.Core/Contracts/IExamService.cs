using QuizExam.Core.Models.Exam;

namespace QuizExam.Core.Contracts
{
    public interface IExamService
    {
        Task<bool> CreateExam(NewExamVM model);
    }
}
