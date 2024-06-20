using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Error;
using QuizExam.Core.Models.Exam;
using System.Diagnostics;

namespace QuizExam.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly IExamService examService;
        private readonly ISubjectService subjectService;

        public HomeController(
            ILogger<HomeController> logger,
            IExamService examService,
            ISubjectService subjectService)
        {
            this.logger = logger;
            this.examService = examService;
            this.subjectService = subjectService;
        }

        public async Task<IActionResult> Index(string? subjectId = null, string? examTitle = null)
        {
            var subjects = await this.subjectService.GetActiveSubjectsAsync();
            var exams = await this.examService.GetActiveExamsAsync(subjectId, examTitle);

            ViewBag.Subjects = subjects
                .Select(s => new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                })
                .ToList();

            FilterExamsVM model = new FilterExamsVM
            {
                SubjectId = subjectId,
                Exams = exams
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}