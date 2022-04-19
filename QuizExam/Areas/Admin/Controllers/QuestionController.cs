using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizExam.Infrastructure.Data.Enums;

namespace QuizExam.Areas.Admin.Controllers
{
    public class QuestionController : BaseController
    {
        public IActionResult QuestionType(string id)
        {
            ViewBag.CountOptions = Enumerable.Range(2, 7)
                .Select(i => new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });

            ViewBag.QuestionTypes = Enum.GetValues(typeof(QuestionTypeEnum))
                .Cast<QuestionTypeEnum>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = v.ToString()
                });

            return View("AnswerOptionsCount");
        }
    }
}
