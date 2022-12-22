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

        public IActionResult New()
        {
            return View("New");
        }

        [HttpPost]
        public async Task<IActionResult> New(NewSubjectVM model)
        {
            try
            {
                await this.subjectService.CreateAsync(model);
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedSubjectMessage;
                
                return RedirectToAction(nameof(GetSubjectsList));
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulCreationMessage;
                return RedirectToAction(nameof(GetSubjectsList));
            }
        }

        public async Task<IActionResult> GetSubjectsList()
        {
            try
            {
                var subjects = await this.subjectService.GetAllSubjectsAsync();

                return View("SubjectsList", subjects);
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var subject = await this.subjectService.GetSubjectForEditAsync(id);

                if (!String.IsNullOrEmpty(subject.Id))
                {
                    return View("Edit", subject);
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return RedirectToAction(nameof(GetSubjectsList));
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction(nameof(GetSubjectsList));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NewSubjectVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (await this.subjectService.EditAsync(model))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulEditMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEditMessage;
                }

                return RedirectToAction(nameof(GetSubjectsList));
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEditMessage;
                return RedirectToAction(nameof(GetSubjectsList));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Activate(string id)
        {
            try
            {
                if (await this.subjectService.ActivateAsync(id))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulActivationMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulActivationMessage;
                }

                return RedirectToAction(nameof(GetSubjectsList));
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulActivationMessage;
                return RedirectToAction(nameof(GetSubjectsList));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(string id)
        {
            try
            {
                if (await this.subjectService.DeactivateAsync(id))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulDeactivationMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeactivationMessage;
                }

                return RedirectToAction(nameof(GetSubjectsList));
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeactivationMessage;
                return RedirectToAction(nameof(GetSubjectsList));
            }
        }
    }
}
