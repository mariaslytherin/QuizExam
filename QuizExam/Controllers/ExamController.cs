using Microsoft.AspNetCore.Mvc;

namespace QuizExam.Controllers
{
    public class ExamController : Controller
    {
        public ExamController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
