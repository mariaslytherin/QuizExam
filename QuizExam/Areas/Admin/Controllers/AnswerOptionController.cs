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

        public async Task<IActionResult> NewOption(string id, string examId)
        {
            var options = this.answerService.GetOptions(id);
            TempData["ExamId"] = examId;

            return View("QuestionAnswerOption");
        }

        [HttpPost]
        public async Task<IActionResult> NewOption(AddAnswerOptionVM model, string examId)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfullAddOptionMessage;
                return View("QuestionAnswerOption");
            }

            if (await this.answerService.Create(model))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedOptionMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = examId });
        }

        public async Task<IActionResult> SetCorrectAnswers(string id, string examId)
        {
            var options = this.answerService.GetOptions(id);
            var question = await this.questionService.GetQuestionById(id);
            TempData["ExamId"] = examId;
            TempData["QuestionContent"] = question.Content;

            ViewBag.Options = options;

            return View("SetCorrectAnswerOptions");
        }

        [HttpPost]
        public async Task<IActionResult> SetCorrectAnswers(QuestionExamVM model, string examId)
        {
            TempData["ExamId"] = examId;

            return View("QuestionAnswerOption");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string questionId, string examId)
        {
            if (await this.answerService.Delete(id))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyDeletedOptionMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction("Edit", "Question", new { id = questionId, examId = examId });
        }
    }
}
