using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models;
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
            var user = await this.userService.GetUserForEdit(id);

            return View("Edit", user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await this.userService.EditUserData(model))
            {
                
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return View("Edit", model);
        }

        public async Task<IActionResult> Roles(string id)
        {
            var user = await this.userService.GetUserById(id);
            var model = new UserRolesVM()
            {
                UserId = id,
                Name = $"{user.FirstName} {user.LastName}",
            };

            ViewBag.RoleItems = this.roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Id,
                    Selected = this.userManager.IsInRoleAsync(user, r.Name).Result
                });

            return View(model);
        }

        public async Task<IActionResult> CreateRole()
        {
            // await roleManager.CreateAsync(new IdentityRole()
            // {
            //     Name = "Student"
            // });

            return Ok();
        }
    }
}
