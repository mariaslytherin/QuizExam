using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Extensions;
using QuizExam.Core.Models.Exam;

namespace QuizExam.Areas.Admin.Controllers
{
    public class ExamController : BaseController
    {
        private readonly IExamService examService;
        private readonly ISubjectService subjectService;

        public ExamController(
            IExamService examService,
            ISubjectService subjectService)
        {
            this.examService = examService;
            this.subjectService = subjectService;
        }

        public async Task<IActionResult> GetExamsList(int p = 1, int s = 10)
        {
            try
            {
                var exams = await this.examService.GetAllExamsAsync(p, s);

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
                return View();
            }

            try
            {
                await this.examService.CreateAsync(model);
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulCreateMessage;
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
