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
            return View("New");
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
            if (TempData[MessageConstant.SuccessMessage] != null)
            {
                ViewData[MessageConstant.SuccessMessage] = TempData[MessageConstant.SuccessMessage]?.ToString();
            }
            if (TempData[MessageConstant.SuccesfulEditMessage] != null)
            {
                ViewData[MessageConstant.SuccessMessage] = TempData[MessageConstant.SuccesfulEditMessage]?.ToString();
            }

            var subjects = await this.subjectService.GetAllSubjects();

            return View("SubjectsList", subjects);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var subject = await this.subjectService.GetSubjectForEdit(Guid.Parse(id));

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
                TempData[MessageConstant.SuccesfulEditMessage] = MessageConstant.SuccesfulEditMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetSubjectsList));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(string id)
        {
            if (await this.subjectService.Activate(Guid.Parse(id)))
            {
                TempData[MessageConstant.SuccessMessage] = "Успешно активиране!";
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
            if (await this.subjectService.Deactivate(Guid.Parse(id)))
            {
                TempData[MessageConstant.SuccessMessage] = "Успешно деактивиране!";
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetSubjectsList));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (await this.subjectService.Delete(Guid.Parse(id)))
            {
                TempData[MessageConstant.SuccessMessage] = "Успешно изтриване!";
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetSubjectsList));
        }
    }
}
