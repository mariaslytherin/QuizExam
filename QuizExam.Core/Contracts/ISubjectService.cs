using QuizExam.Core.Models.Subject;

namespace QuizExam.Core.Contracts
{
    public interface ISubjectService
    {
        Task AddSubject(SubjectVM model);

        Task<IEnumerable<SubjectVM>> GetAllSubjects();
    }
}
