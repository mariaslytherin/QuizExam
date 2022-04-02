using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Infrastructure.Data.Identity;

namespace QuizExam.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
