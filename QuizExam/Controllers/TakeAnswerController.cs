using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.TakeQuestion;

namespace QuizExam.Controllers
{
    public class TakeAnswerController : Controller
    {
        private readonly ITakeAnswerService takeAnswerService;

        public TakeAnswerController(
            ITakeAnswerService takeAnswerService)
        {
            this.takeAnswerService = takeAnswerService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(TakeQuestionVM model, string examId)
        {
            try
            {
                if (model.CheckedOptionId == null)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckAnswerMessage;
                    return RedirectToAction("GetNextQuestion", "Question", new { takeId = model.TakeExamId, examId = examId, order = model.Order });
                }

                if (await this.takeAnswerService.AddAnswer(model, examId))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulRecordMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return RedirectToAction("Index", "Home");
                }

                TempData["ExamId"] = examId;

                if (model.IsLast)
                {
                    return RedirectToAction("Finish", "TakeExam", new { takeExamId = model.TakeExamId });
                }
                else
                {
                    return RedirectToAction("GetNextQuestion", "Question", new { takeId = model.TakeExamId, examId, order = model.Order + 1 });
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
