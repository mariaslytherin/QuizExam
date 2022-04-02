using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models;
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

        public async Task<IEnumerable<UserListVM>> GetAllUsers()
        {
            return await this.repository.All<ApplicationUser>()
                .Select(u => new UserListVM()
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                })
                .ToListAsync();
        }

        public async Task<UserEditVM> GetUser(string id)
        {
            var user = await this.repository.GetByIdAsync<ApplicationUser>(id);

            return new UserEditVM() 
            { 
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }
    }
}
