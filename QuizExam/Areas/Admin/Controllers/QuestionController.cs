using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Question;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Areas.Admin.Controllers
{
    public class QuestionController : BaseController
    {
        private readonly IQuestionService questionService;
        private readonly IAnswerOptionService answerOptionService;
        private readonly IExamService examService;

        public QuestionController(
            IQuestionService questionService,
            IAnswerOptionService answerOptionService,
            IExamService examService)
        {
            this.questionService = questionService;
            this.answerOptionService = answerOptionService;
            this.examService = examService;
        }

        public IActionResult New(string id, string isActive)
        {
            if (bool.Parse(isActive))
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamMustNotBeActive;
                return RedirectToAction("ViewExam", "Exam", new { id = id });
            }

            return View("New");
        }

        [HttpPost]
        public async Task<IActionResult> New(NewQuestionVM model, string examId)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulCreateMessage;
                return View(model);
            }

            try
            {
                var questionId = await this.questionService.CreateAsync(model);

                if (questionId != Guid.Empty)
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulCreateMessage;
                    TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningAddOptionsMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulCreateMessage;
                    return View(model);
                }

                return RedirectToAction("Edit", new { id = questionId, examId });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulCreateMessage;
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(string examId, string id)
        {
            try
            {
                var isExamDeactivated = await this.examService.IsExamDeactivatedAsync(examId);
                if (!isExamDeactivated)
                {
                    TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningExamIsActiveEditQuestionMessage;
                }

                var question = await this.questionService.GetQuestionForEditAsync(id);
                var options = await this.answerOptionService.GetOptionsAsync(id);

                if (!string.IsNullOrEmpty(question.Id) && options != Enumerable.Empty<AnswerOptionVM>())
                {
                    question.ExamId = examId;
                    if (options.Count() != 0)
                    {
                        ViewBag.Options = options;
                    }

                    return View("Edit", question);
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulCreateMessage;
                    return RedirectToAction("ViewExam", "Exam", new { id = examId });
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditQuestionVM model, string examId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEditMessage;
                    return RedirectToAction("Edit", new { examId, id = model.Id });
                }

                var isExamDeactivated = await this.examService.IsExamDeactivatedAsync(examId);
                if (!isExamDeactivated)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamMustBeDeactivatedToEdit;
                    return RedirectToAction("Edit", new { examId, id = model.Id });
                }

                if (!await this.questionService.HasEnoughAnswerOptionsAsync(model.Id))
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorNotEnoughAnswerOptionsMessage;
                    return RedirectToAction("Edit", new { examId, id = model.Id });
                }

                if (!await this.questionService.HasCorrectAnswerAsync(model.Id))
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorNotSelectedCorrectAnswerMessage;
                    return RedirectToAction("Edit", new { examId, id = model.Id });
                }

                if (await this.questionService.EditAsync(model))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulEditMessage;
                    return RedirectToAction("ViewExam", "Exam", new { id = examId });
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEditMessage;
                    return RedirectToAction("Edit", new { examId, id = model.Id });
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEditMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string examId)
        {
            try
            {
                var isExamDeactivated = await this.examService.IsExamDeactivatedAsync(examId);
                if (!isExamDeactivated)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamMustBeDeactivatedToEdit;
                    return RedirectToAction("ViewExam", "Exam", new { id = examId });
                }

                if (await this.questionService.DeleteAsync(id))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulDeleteMessage;
                    return RedirectToAction("ViewExam", "Exam", new { id = examId });
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeleteMessage;
                    return RedirectToAction("ViewExam", "Exam", new { id = examId });
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeleteMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }
    }
}
