using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Question;

namespace QuizExam.Controllers
{
    public class StatisticsController : BaseController
    {
        private readonly IExamService examService;
        private readonly ISubjectService subjectService;

        public StatisticsController(IExamService examService, ISubjectService subjectService)
        {
            this.examService = examService;
            this.subjectService = subjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTopHardestQuestions(string subjectId = null, string examId = null)
        {
            var subjects = await this.subjectService.GetAllSubjectsAsync();
            var exams = await this.examService.GetActiveExamsBySubjectAsync(subjectId);

            ViewBag.Subjects = subjects
                .Select(s => new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                })
                .ToList();

            ViewBag.Exams = exams
                .Select(s => new SelectListItem()
                {
                    Text = s.Title,
                    Value = s.Id.ToString(),
                })
                .ToList();

            var hardestQuestions = this.examService.GetExamTop5HardestQuestionsAsync(examId);
            var model = new HardestQuestionVM
            {
                SubjectId = subjectId,
                ExamId = examId,
                HardestQuestionsInfo = hardestQuestions,
            };

            return View("ExamHardestQuestions", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetExamsForSubject(string subjectId)
        {
            var exams = await this.examService.GetActiveExamsBySubjectAsync(subjectId);
            return Json(exams);
        }
    }
}
