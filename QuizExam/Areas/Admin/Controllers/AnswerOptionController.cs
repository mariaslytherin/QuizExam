using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Question;

namespace QuizExam.Areas.Admin.Controllers
{
    public class AnswerOptionController : BaseController
    {
        private readonly IAnswerOptionService answerService;
        private readonly IQuestionService questionService;

        public AnswerOptionController(
            IAnswerOptionService answerService,
            IQuestionService questionService)
        {
            this.answerService = answerService;
            this.questionService = questionService;
        }

        public async Task<IActionResult> New(string questionId, string examId)
        {
            try
            {
                var options = await this.answerService.GetOptionsAsync(questionId);

                if (options == Enumerable.Empty<AnswerOptionVM>())
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return RedirectToAction("Edit", "Question", new { id = questionId, examId });
                }
                else if (options.ToList().Count == 6)
                {
                    TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningCannotAddOptionMessage;
                    return RedirectToAction("Edit", "Question", new { id = questionId, examId });
                }
                else
                {
                    TempData["ExamId"] = examId;
                    TempData["QuestionId"] = questionId;

                    return View("QuestionAnswerOption");
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> New(NewAnswerOptionVM model, string examId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfullAddMessage;
                    return View("QuestionAnswerOption");
                }

                if (await this.answerService.CreateAsync(model))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulAddMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfullAddMessage;
                }

                return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfullAddMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }

        public async Task<IActionResult> SetCorrectAnswer(string id, string examId)
        {
            try
            {
                var options = await this.answerService.GetOptionsAsync(id);
                var question = await this.questionService.GetQuestionByIdAsync(id);

                if (options.Count() < 2)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorNotEnoughAnswerOptionsMessage;
                    return RedirectToAction("Edit", "Question", new { id, examId });
                }

                if (options != Enumerable.Empty<AnswerOptionVM>() || question != null)
                {
                    TempData["ExamId"] = examId;
                    TempData["QuestionContent"] = question.Content;

                    var model = new SetCorrectAnswerVM
                    {
                        QuestionId = question.Id.ToString(),
                        Options = options.ToList(),
                    };

                    return View("SetCorrectAnswerOptions", model);
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return RedirectToAction("Edit", "Question", new { id, examId });
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Edit", "Question", new { id, examId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetCorrectAnswer(SetCorrectAnswerVM model, string examId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckAnswerMessage;
                    return RedirectToAction("SetCorrectAnswer", new { id = model.QuestionId, examId });
                }

                if (model.CorrectAnswerId == null)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckAnswerMessage;
                    return RedirectToAction("SetCorrectAnswer", new { id = model.QuestionId, examId });
                }

                if (await this.answerService.SetCorrectAnswerAsync(model))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulRecordMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                }

                TempData["ExamId"] = examId;

                return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string questionId, string examId)
        {
            try
            {
                if (await this.answerService.DeleteAsync(id))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulDeleteMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeleteMessage;
                }

                return RedirectToAction("Edit", "Question", new { id = questionId, examId });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeleteMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }
    }
}
