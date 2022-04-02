using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Infrastructure.Data.Identity;

namespace QuizExam.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public UserController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await this.userService.GetAllUsers();

            return View("UsersList", users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.userService.GetUser(id);

            return View("Edit", user);
        }

        public async Task<IActionResult> Roles(string id)
        {
            return Ok(id);
        }

        public async Task<IActionResult> CreateRole()
        {
            // await roleManager.CreateAsync(new IdentityRole()
            // {
            //     Name = "Administrator"
            // });

            return Ok();
        }
    }
}
