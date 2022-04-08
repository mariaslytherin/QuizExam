using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Subject;
using QuizExam.Infrastructure.Data;

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

        [HttpGet]
        public IActionResult NewSubject()
        {
            return View("NewSubject");
        }

        [HttpPost]
        public async Task<IActionResult> NewSubject(SubjectVM model)
        {
            if (await this.subjectService.AddSubject(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "Успешен запис!";
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetSubjectsList));
        }

        public async Task<IActionResult> GetSubjectsList()
        {
            var subjects = await this.subjectService.GetAllSubjects();

            return View("SubjectsList", subjects);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var subject = await this.subjectService.GetSubjectForEdit(id);

            return View("Edit", subject);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubjectVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await this.subjectService.Edit(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "Успешен запис!";
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetSubjectsList));
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(string id)
        {
            if (await this.subjectService.Deactivate(id))
            {
                ViewData[MessageConstant.SuccessMessage] = "Успешно деактивиране!";
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetSubjectsList));
        }
    }
}
