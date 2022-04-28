using Microsoft.AspNetCore.Mvc;

namespace QuizExam.Controllers
{
    public class ExamController : BaseController
    {
        public IActionResult Start()
        {
            return View("Take");
        }
    }
}
