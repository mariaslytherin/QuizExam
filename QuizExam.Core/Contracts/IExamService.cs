using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.Question;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface IExamService
    {
        Task CreateAsync(string userId, NewExamVM model);

        Task<ExamListVM> GetAllExamsAsync(string userId, bool isSuperAdmin, int? page, int? size);

        Task<List<ViewExamVM>> GetExamsForUserAsync();

        Task<ExamVM> GetExamInfoAsync(string id);

        Task<EditExamVM> GetExamForEditAsync(string id);

        Task<ViewExamVM> GetExamForViewAsync(string id);

        List<HardestQuestionInfoVM> GetExamTop5HardestQuestionsAsync(string examId);

        Task<IEnumerable<Exam>> GetActiveExamsBySubjectAsync(string subjectId);

        Task<bool> EditAsync(EditExamVM model);

        Task<bool> DeactivateAsync(string id);

        Task<bool> ActivateAsync(string id);

        Task<bool> DeleteAsync(string id);

        Task<bool> IsExamDeactivated(string id);

        Task<bool> HasAnyQuestionsAsync(string id);

        Task<bool> QuestionsPointsSumEqualsMaxScoreAsync(string id);

        Task<bool> HasQuestionsWithoutSetCorrectAnswerAsync(string id);
    }
}
