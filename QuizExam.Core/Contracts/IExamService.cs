using QuizExam.Core.Models.Exam;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface IExamService
    {
        Task CreateAsync(NewExamVM model);

        Task<ExamListVM> GetAllExamsAsync(int? page, int? size);

        Task<List<ViewExamVM>> GetExamsForUserAsync();

        Task<ExamVM> GetExamInfoAsync(string id);

        Task<EditExamVM> GetExamForEditAsync(string id);

        Task<ViewExamVM> GetExamForViewAsync(string id);

        Task<bool> EditAsync(EditExamVM model);

        Task<bool> DeactivateAsync(string id);

        Task<bool> ActivateAsync(string id);

        Task<bool> DeleteAsync(string id);

        Task<bool> HasAnyQuestionsAsync(string id);

        Task<bool> QuestionsPointsSumEqualsMaxScoreAsync(string id);
    }
}
