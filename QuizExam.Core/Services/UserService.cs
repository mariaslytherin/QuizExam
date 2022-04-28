using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models;
using QuizExam.Core.Models.User;
using QuizExam.Infrastructure.Data.Identity;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repository;

        public UserService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> EditUserData(UserEditVM model)
        {
            bool result = false;
            var user = await this.repository.GetByIdAsync<ApplicationUser>(model.Id);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<UserListVM> GetAllUsers(int page, int size)
        {
            var model = new UserListVM()
            {
                PageNo = page,
                PageSize = size
            };

            var users = await this.repository.All<ApplicationUser>()
                .Select(u => new UserVM
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = $"{u.FirstName} {u.LastName}",
                })
                .OrderBy(u => u.Email)
                .Skip(page * size - size)
                .Take(size)
                .ToListAsync();

            model.TotalRecords = await this.repository.All<ApplicationUser>().CountAsync();
            model.Users = users;

            return model;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await this.repository.GetByIdAsync<ApplicationUser>(id);
        }

        public async Task<UserEditVM> GetUserForEdit(string id)
        {
            var user = await this.repository.GetByIdAsync<ApplicationUser>(id);

            return new UserEditVM() 
            { 
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }
    }
}
