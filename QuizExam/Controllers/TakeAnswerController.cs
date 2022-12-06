using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.TakeQuestion;

namespace QuizExam.Controllers
{
    public class TakeAnswerController : Controller
    {
        private readonly ITakeAnswerService takeAnswerService;
        private readonly IQuestionService questionService;

        public TakeAnswerController(
            ITakeAnswerService takeAnswerService,
            IQuestionService questionService)
        {
            this.takeAnswerService = takeAnswerService;
            this.questionService = questionService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(TakeQuestionVM model, string examId)
        {
            if (model.CheckedOptionId == null)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckAnswerMessage;
                return RedirectToAction("GetNextQuestion", "Question", new { takeId = model.TakeExamId, examId = examId, order = model.Order });
            }

            if (await this.takeAnswerService.AddAnswer(model, examId))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedCorrectAnswerMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            TempData["ExamId"] = examId;

            if (model.IsLast)
            {
                return RedirectToAction("Finish", "TakeExam", new { takeExamId = model.TakeExamId });
            }
            else
            {
                return RedirectToAction("GetNextQuestion", "Question", new { takeId = model.TakeExamId, examId = examId, order = model.Order + 1 });
            }
        }
    }
}
