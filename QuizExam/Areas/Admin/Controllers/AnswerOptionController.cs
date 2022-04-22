using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;

namespace QuizExam.Areas.Admin.Controllers
{
    public class AnswerOptionController : BaseController
    {
        private readonly IAnswerOptionService answerService;

        public AnswerOptionController(IAnswerOptionService answerService)
        {
            this.answerService = answerService;
        }

        public async Task<IActionResult> NewOption()
        {
            return View("QuestionAnswerOption");
        }

        [HttpPost]
        public async Task<IActionResult> NewOption(QuestionAnswerOptionVM model)
        {
            //TODO
            return View("QuestionAnswerOption");
        }
    }
}
