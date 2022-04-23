using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;

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

        public async Task<IActionResult> NewOption(string id)
        {
            var test = this.questionService.HasAnswerOptions(id);

            return View("QuestionAnswerOption");
        }

        [HttpPost]
        public async Task<IActionResult> NewOption(AddAnswerOptionVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessageConstants.UnsuccessfulAddQuestionMessage] = ErrorMessageConstants.UnsuccessfulAddQuestionMessage;
                return View(model);
            }

            if (await this.answerService.Create(model))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedQuestionMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            

            return RedirectToAction("NewOption", new { id = model.QuestionId });
        }
    }
}
