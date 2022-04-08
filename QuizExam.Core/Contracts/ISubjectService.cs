using QuizExam.Core.Models.Subject;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface ISubjectService
    {
        Task<bool> AddSubject(SubjectVM model);

        Task<IEnumerable<SubjectVM>> GetAllSubjects();

        Task<Subject> GetSubjectById(string id);

        Task<SubjectVM> GetSubjectForEdit(string id);

        Task<bool> Edit(SubjectVM model);

        Task<bool> Deactivate(string id);
    }
}
