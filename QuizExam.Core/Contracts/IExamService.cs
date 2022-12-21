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

        Task<EditExamVM> GetExamForEdit(Guid id);

        Task<ViewExamVM> GetExamForView(string id);

        Task<bool> Edit(EditExamVM model);

        Task<bool> Deactivate(Guid id);

        Task<bool> Activate(Guid id);

        Task<bool> Delete(Guid id);

        Task<bool> CanActivate(Guid id);
    }
}
