using QuizExam.Core.Models.Subject;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface ISubjectService
    {
        Task<bool> AddSubject(SubjectVM model);

        Task<IEnumerable<SubjectVM>> GetAllSubjects();

        Task<IEnumerable<SubjectVM>> GetActiveSubjects();

        Task<SubjectVM> GetSubjectForEdit(Guid id);

        Task<bool> Edit(SubjectVM model);

        Task<bool> Deactivate(Guid id);

        Task<bool> Activate(Guid id);
    }
}
