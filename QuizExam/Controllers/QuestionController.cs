using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.TakeAnswer;
using QuizExam.Core.Models.TakeQuestion;

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

        public async Task<IActionResult> GetQuestion(string questionId, string examId, string takeId, int order)
        {
            try
            {
                var question = await this.questionService.GetQuestionForTake(examId, takeId, order, isLast);

                if (question != null)
                {
                    return View("/Views/TakeExam/Take.cshtml", question);
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return View("Index", "Home");
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return View("Index", "Home");
            }
        }
    }
}
