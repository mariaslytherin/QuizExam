using Microsoft.AspNetCore.Mvc;

namespace QuizExam.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
