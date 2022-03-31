using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;

namespace QuizExam.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = UserRolesConstants.Administrator)]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok();
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
