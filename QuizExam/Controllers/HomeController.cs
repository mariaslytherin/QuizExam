using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Contracts;
using QuizExam.Models;
using System.Diagnostics;

namespace QuizExam.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly IExamService examService;

        public HomeController(
            ILogger<HomeController> logger,
            IExamService examService)
        {
            this.logger = logger;
            this.examService = examService;
        }

        public async Task<IActionResult> Index()
        {
            var exams = await this.examService.GetAllExams(null, null);
            
            return View(exams);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}