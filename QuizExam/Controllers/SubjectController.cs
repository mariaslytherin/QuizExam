using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Subject;

namespace QuizExam.Controllers
{
    public class SubjectController : BaseController
    {
        private readonly ISubjectService subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> NewSubject(SubjectVM model)
        {
            return Ok();
        }

        public async Task<IActionResult> GetSubjectsList()
        {
            var subjects = await this.subjectService.GetAllSubjects();

            return View("SubjectsList", subjects);
        }
    }
}
