using QuizExam.Core.Models.Exam;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface IExamService
    {
        Task<bool> Create(NewExamVM model);

        Task<IEnumerable<ExamListVM>> GetAllExams();

        Task<Exam> GetExamById(Guid id);

        Task<EditExamVM> GetExamForEdit(Guid id);

        Task<ViewExamVM> GetExamForView(string id);

        Task<bool> Edit(EditExamVM model);

        Task<bool> Deactivate(Guid id);

        Task<bool> Activate(Guid id);

        Task<bool> Delete(Guid id);
    }
}
