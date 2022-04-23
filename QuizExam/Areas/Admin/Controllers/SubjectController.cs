using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Subject;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Areas.Admin.Controllers
{
    public class SubjectController : BaseController
    {
        private readonly ISubjectService subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        public IActionResult NewSubject()
        {
            return View("New");
        }

        [HttpPost]
        public async Task<IActionResult> NewSubject(SubjectVM model)
        {
            if (await this.subjectService.AddSubject(model))
            {
                ViewData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedSubjectMessage;
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
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulEditMessage;
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
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulActivationMessage;
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
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulDeactivationMessage;
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
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyDeletedSubjectMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetSubjectsList));
        }
    }
}
