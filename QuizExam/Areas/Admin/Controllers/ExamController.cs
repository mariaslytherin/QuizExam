using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
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
            var exams = await this.examService.GetAllExams(p, s);

            return View("ExamsList", exams);
        }

        public async Task<IActionResult> ViewExam(string id)
        {
            try
            {
                var exam = await this.examService.GetExamForView(id);

                return View("View", exam);

            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamNotFoundMessage;
                return RedirectToAction(nameof(GetExamsList));
            }
        }

        public async Task<IActionResult> NewAsync()
        {
            var subjects = await this.subjectService.GetActiveSubjects();

            ViewBag.Subjects = subjects
                .Select(s => new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                })
                .ToList();

            return View("New");
        }

        [HttpPost]
        public async Task<IActionResult> New(NewExamVM model)
        {
            if (await this.examService.Create(model))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedExamMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetExamsList));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var exam = await this.examService.GetExamForEdit(Guid.Parse(id));

            return View("Edit", exam);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditExamVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await this.examService.Edit(model))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulEditMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetExamsList));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(string id)
        {
            try
            {
                if (!await this.examService.CanActivate(Guid.Parse(id)))
                {
                    TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningActivationMessage;
                    return RedirectToAction(nameof(GetExamsList));
                }

                if (await this.examService.Activate(Guid.Parse(id)))
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
            if (await this.examService.Deactivate(Guid.Parse(id)))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulDeactivationMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetExamsList));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (await this.examService.Delete(Guid.Parse(id)))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = "Успешно изтриване!";
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction(nameof(GetExamsList));
        }
    }
}
