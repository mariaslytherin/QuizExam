using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Exam;

namespace QuizExam.Controllers
{
    public class ExamController : BaseController
    {
        private readonly IExamService examService;

        public ExamController(IExamService examService)
        {
            this.examService = examService;
        }

        [HttpGet]
        public IActionResult New()
        {
            return View("New");
        }

        [HttpPost]
        public async Task<IActionResult> New(NewExamVM model)
        {
            if (await this.examService.CreateExam(model))
            {
                ViewData[MessageConstants.SuccessMessage] = MessageConstants.SuccessfullyAddedExamMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return Ok();
        }
    }
}
