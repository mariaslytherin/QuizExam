using QuizExam.Core.Models;
using QuizExam.Infrastructure.Data.Identity;

namespace QuizExam.Core.Contracts
{
    public interface IUserService
    {
        Task<UserListVM> GetAllUsers(int p, int s);

        Task<UserEditVM> GetUserForEdit(string id);

        Task<bool> EditUserData(UserEditVM model);

        Task<ApplicationUser> GetUserById(string id);
    }
}
