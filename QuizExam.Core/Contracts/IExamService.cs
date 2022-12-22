using QuizExam.Core.Models.Exam;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface IExamService
    {
        Task Create(NewExamVM model);

        Task<ExamListVM> GetAllExams(int? page, int? size);

        Task<List<ViewExamVM>> GetExamsForUser();

        Task<ExamVM> GetExamInfo(string id);

        Task<EditExamVM> GetExamForEdit(string id);

        Task<ViewExamVM> GetExamForView(string id);

        Task<bool> Edit(EditExamVM model);

        Task<bool> Deactivate(string id);

        Task<bool> Activate(string id);

        Task<bool> Delete(string id);

        Task<bool> HasAnyQuestions(string id);

        Task<bool> QuestionsPointsSumEqualsMaxScore(string id);
    }
}
