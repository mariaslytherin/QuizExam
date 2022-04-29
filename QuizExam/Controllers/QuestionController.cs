using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.TakeAnswer;
using QuizExam.Core.Models.TakeQuestion;

namespace QuizExam.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionService questionService;
        private readonly IAnswerOptionService answerService;

        public QuestionController(
            IQuestionService questionService,
            IAnswerOptionService answerService)
        {
            this.questionService = questionService;
            this.answerService = answerService;
        }

        public async Task<IActionResult> GetQuestion(string questionId, string examId, string takeId, int order)
        {
            try
            {
                var question = await this.questionService.GetQuestionForTake(examId, order);

                if (question != null)
                {
                    TempData["QuestionContent"] = question.Content;
                    TempData["ExamId"] = question.ExamId;

                    var options = this.answerService.GetOptions(question.Id.ToString());

                    var model = new TakeQuestionVM();
                    model.QuestionId = question.Id.ToString();
                    model.TakeExamId = takeId;
                    model.Order = order;
                    model.TakeAnswers = new List<TakeAnswerVM>();

                    foreach (var option in options)
                    {
                        var currentOption = new TakeAnswerVM
                        {
                            AnswerId = option.Id,
                            Content = option.Content,
                        };
                        model.TakeAnswers.Add(currentOption);
                    }

                    return View("/Views/TakeExam/Take.cshtml", model);
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return View("Index", "Home");
            }
        }
    }
}
