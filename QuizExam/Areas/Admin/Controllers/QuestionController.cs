using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Question;

namespace QuizExam.Areas.Admin.Controllers
{
    public class QuestionController : BaseController
    {
        private readonly IQuestionService questionService;
        private readonly IAnswerOptionService answerOptionService;

        public QuestionController(IQuestionService questionService, IAnswerOptionService answerOptionService)
        {
            this.questionService = questionService;
            this.answerOptionService = answerOptionService;
        }

        public IActionResult New()
        {
            return View("New");
        }

        [HttpPost]
        public async Task<IActionResult> New(NewQuestionVM model, string examId)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulAddQuestionMessage;
                return View(model);
            }

            try
            {
                var questionId = await this.questionService.CreateAsync(model);

                if (questionId != Guid.Empty)
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedQuestionMessage;
                    TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningAddOptionsMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulAddQuestionMessage;
                    return View(model);
                }

                return RedirectToAction("Edit", new { id = questionId, examId = examId });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulAddQuestionMessage;
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(string examId, string id)
        {
            try
            {
                var question = await this.questionService.GetQuestionForEditAsync(id);
                var options = await this.answerOptionService.GetOptions(id);

                if (!string.IsNullOrEmpty(question.Id) && options != Enumerable.Empty<AnswerOptionVM>())
                {
                    TempData["ExamId"] = examId;
                    
                    if (options.Count() != 0)
                    {
                        ViewBag.Options = options;
                    }

                    return View("Edit", question);
                }
                else
                {
                    TempData[ErrorMessageConstants.UnsuccessfulAddQuestionMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return RedirectToAction("ViewExam", "Exam", new { id = examId });
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.UnsuccessfulAddQuestionMessage] = ErrorMessageConstants.ErrorAppeardMessage;
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
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEdit;
                    return View(model);
                }

                if (!await this.questionService.HasEnoughAnswerOptions(model.Id))
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorNotEnoughAnswerOptionsMessage;
                    return View(model);
                }

                if (await this.questionService.EditAsync(model))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulEditMessage;
                    return RedirectToAction("ViewExam", "Exam", new { id = examId });
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEdit;
                    return View(model);
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEdit;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string examId)
        {
            try
            {
                if (await this.questionService.DeleteAsync(id))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyDeletedQuestionMessage;
                    return RedirectToAction("ViewExam", "Exam", new { id = examId });
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeletionMessage;
                    return RedirectToAction("ViewExam", "Exam", new { id = examId });
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeletionMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }
    }
}
