using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Infrastructure.Data.Enums;

namespace QuizExam.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionService questionService;
        private readonly IAnswerOptionService answerService;

        public QuestionController(
            IQuestionService questionService,
            IAnswerOptionService answerService)
        {
            this.questionService = questionService;
            this.answerService = answerService;
        }

        public async Task<IActionResult> GetQuestion(string examId, string takeId, int order)
        {
            try
            {
                var question = await this.questionService.GetNextQuestionAsync(examId, takeId, order);

                if (!String.IsNullOrEmpty(question.QuestionId))
                {
                    return View("/Views/TakeExam/Take.cshtml", question);
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return Ok(new { errorMessage = ErrorMessageConstants.ErrorAppeardMessage });
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> GetNextQuestion(string examId, string takeId, int order)
        {
            try
            {
                var question = await this.questionService.GetNextQuestionAsync(examId, takeId, order);

                if (!String.IsNullOrEmpty(question.QuestionId))
                {
                    return PartialView("/Views/Shared/_TakePartial.cshtml", question);
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return Ok(new { errorMessage = ErrorMessageConstants.ErrorAppeardMessage });
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> GetPreviousQuestion(string examId, string takeId, int order)
        {
            try
            {
                var question = await this.questionService.GetPreviousQuestionAsync(examId, takeId, order);

                if (question != null)
                {
                    return PartialView("/Views/Shared/_TakePartial.cshtml", question);
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
