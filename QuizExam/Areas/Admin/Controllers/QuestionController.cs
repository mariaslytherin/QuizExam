using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Question;
using QuizExam.Infrastructure.Data.Enums;

namespace QuizExam.Areas.Admin.Controllers
{
    public class QuestionController : BaseController
    {
        private readonly IQuestionService questionService;

        public QuestionController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        public async Task<IActionResult> New()
        {
            return View("New");
        }

        [HttpPost]
        public async Task<IActionResult> New(NewQuestionVM model)
        {
            if (await this.questionService.Create(model))
            {
                TempData[MessageConstants.SuccessMessage] = MessageConstants.SuccessfullyAddedQuestionMessage;
                TempData["QuestionContent"] = model.Content;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction("NewOption", "AnswerOption", new { area = "Admin" });
        }
    }
}
