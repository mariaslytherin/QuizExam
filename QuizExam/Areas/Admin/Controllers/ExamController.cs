using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.Exam;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Identity;

namespace QuizExam.Areas.Admin.Controllers
{
    public class ExamController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IExamService examService;
        private readonly ISubjectService subjectService;

        public ExamController(
            UserManager<ApplicationUser> userManager,
            IExamService examService,
            ISubjectService subjectService)
        {
            this.userManager = userManager;
            this.examService = examService;
            this.subjectService = subjectService;
        }

        public async Task<IActionResult> GetExamsList(int p = 1, int s = 10)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(User);
                var isSuperAdmin = await this.userManager.IsInRoleAsync(user, UserRolesConstants.SuperAdmin);
                var exams = await this.examService.GetAllExamsAsync(user.Id, isSuperAdmin, p, s);

                return View("ExamsList", exams);
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamNotFoundMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> ViewExam(string id)
        {
            try
            {
                var isExamDeactivated = await this.examService.IsExamDeactivatedAsync(id);
                if (!isExamDeactivated)
                {
                    TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningExamIsActiveMessage;
                }

                var exam = await this.examService.GetExamForViewAsync(id);

                return View("View", exam);

            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamNotFoundMessage;
                return RedirectToAction(nameof(GetExamsList));
            }
        }

        public async Task<IActionResult> New()
        {
            try
            {
                var subjects = await this.subjectService.GetActiveSubjectsAsync();

                ViewBag.Subjects = subjects
                    .Select(s => new SelectListItem()
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                    })
                    .ToList();

                return View("New");
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction(nameof(GetExamsList));
            }
        }

        [HttpPost]
        public async Task<IActionResult> New(NewExamVM model)
        {
            if (!ModelState.IsValid)
            {
                var subjects = await this.subjectService.GetActiveSubjectsAsync();

                ViewBag.Subjects = subjects
                    .Select(s => new SelectListItem()
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                    })
                    .ToList();

                return View();
            }

            try
            {
                var user = await this.userManager.GetUserAsync(User);
                if (user != null)
                {
                    await this.examService.CreateAsync(user.Id, model);
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulCreateMessage;
                    return RedirectToAction(nameof(GetExamsList));
                }

                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulCreateMessage;
                return RedirectToAction(nameof(GetExamsList));
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulCreateMessage;
                return RedirectToAction(nameof(GetExamsList));
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var exam = await this.examService.GetExamForEditAsync(id);

                if (!string.IsNullOrEmpty(exam.Id))
                {
                    return View("Edit", exam);
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamNotFoundMessage;
                    return RedirectToAction(nameof(GetExamsList));
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamNotFoundMessage;
                return RedirectToAction(nameof(GetExamsList));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditExamVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (!await this.examService.IsExamDeactivatedAsync(model.Id))
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamMustBeDeactivatedToEdit;
                    return RedirectToAction(nameof(GetExamsList));
                }

                if (await this.examService.EditAsync(model))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulEditMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEditMessage;
                }

                return RedirectToAction(nameof(GetExamsList));
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEditMessage;
                return RedirectToAction(nameof(GetExamsList));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Activate(string id)
        {
            try
            {
                if (!await this.examService.HasAnyQuestionsAsync(id))
                {
                    TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningExamMissingQuestionsMessage;
                    return RedirectToAction(nameof(GetExamsList));
                }

                if (!await this.examService.QuestionsPointsSumEqualsMaxScoreAsync(id))
                {
                    TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningExamNotEqualPointsMessage;
                    return RedirectToAction(nameof(GetExamsList));
                }

                if (await this.examService.HasQuestionsWithoutSetCorrectAnswerAsync(id))
                {
                    TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningExamHasQuestionsWithoutCorrectAnswerMessage;
                    return RedirectToAction(nameof(GetExamsList));
                }

                if (await this.examService.ActivateAsync(id))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulActivationMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                }

                return RedirectToAction(nameof(GetExamsList));
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction(nameof(GetExamsList));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(string id)
        {
            try
            {
                if (await this.examService.DeactivateAsync(id))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulDeactivationMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                }

                return RedirectToAction(nameof(GetExamsList));
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction(nameof(GetExamsList));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (await this.examService.DeleteAsync(id))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulDeleteMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeleteMessage;
                }

                return RedirectToAction(nameof(GetExamsList));
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeleteMessage;
                return RedirectToAction(nameof(GetExamsList));
            }
        }
    }
}
