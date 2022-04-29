using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.TakeQuestion;

namespace QuizExam.Controllers
{
    public class TakeAnswerController : Controller
    {
        private readonly ITakeAnswerService takeAnswerService;

        public TakeAnswerController(ITakeAnswerService takeAnswerService)
        {
            this.takeAnswerService = takeAnswerService;
        }

        [HttpPost]
        public IActionResult Add(TakeQuestionVM model, string examId)
        {
            var checkedOptions = model.TakeAnswers.Where(o => o.Selected).ToList().Count;

            if (checkedOptions == 0)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckAnswerMessage;
                return RedirectToAction("SetCorrectAnswer", new { id = model.QuestionId, examId = examId });
            }
            else if (checkedOptions > 1)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckOnlyOneMessage;
                return RedirectToAction("SetCorrectAnswer", new { id = model.QuestionId, examId = examId });
            }

            var answer = model.TakeAnswers.Where(a => a.Selected);

            if (true)//await this.takeAnswerService.AddAnswer(takeId, answer))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedCorrectAnswerMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            TempData["ExamId"] = examId;

            return RedirectToAction("GetQuestion", "Question", new { takeId = model.TakeExamId, examId = examId, order = model.Order });
        }
    }
}
