using QuizExam.Core.Models;

namespace QuizExam.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListVM>> GetAllUsers();

        Task<UserEditVM> GetUser(string id);
    }
}
