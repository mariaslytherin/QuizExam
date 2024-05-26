using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Contracts;
using QuizExam.Infrastructure.Data;

namespace QuizExam.Controllers
{
    public class StatisticsController : BaseController
    {
        private readonly IExamService examService;

        public StatisticsController(IExamService examService)
        {
            this.examService = examService;
        }

        public IActionResult GetTopHardestQuestions(string? examId = null, int? topN = null)
        {
            var hardestQuestions = this.examService.GetExamTopHardestQuestionsAsync("D18715F0-CE0B-4C2E-AF72-999CDBA54EC5", 5);

            return View("ExamHardestQuestions", hardestQuestions);
        }
    }
}
