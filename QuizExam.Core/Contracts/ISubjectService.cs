using QuizExam.Core.Models.Subject;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Core.Contracts
{
    public interface ISubjectService
    {
        Task CreateAsync(NewSubjectVM model);

        Task<IList<SubjectVM>> GetAllSubjectsAsync();

        Task<IEnumerable<SubjectVM>> GetActiveSubjectsAsync();

        Task<NewSubjectVM> GetSubjectForEditAsync(string id);

        Task<bool> EditAsync(NewSubjectVM model);

        Task<bool> DeactivateAsync(string id);

        Task<bool> ActivateAsync(string id);
    }
}
