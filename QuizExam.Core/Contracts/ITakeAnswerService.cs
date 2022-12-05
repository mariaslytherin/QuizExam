using QuizExam.Core.Models.TakeAnswer;
using QuizExam.Core.Models.TakeQuestion;

namespace QuizExam.Core.Contracts
{
    public interface ITakeAnswerService
    {
        Task<bool> AddAnswer(TakeQuestionVM model, string examId);

        Task<bool> DeleteAnswer(TakeQuestionVM model, string examId);
    }
}
