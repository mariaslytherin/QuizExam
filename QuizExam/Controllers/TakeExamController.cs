using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Exam;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Identity;

namespace QuizExam.Controllers
{
    public class TakeExamController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITakeExamService takeExamService;
        private readonly IExamService examService;
        private readonly IQuestionService questionService;

        public TakeExamController(
            UserManager<ApplicationUser> userManager,
            ITakeExamService takeExamService,
            IExamService examService,
            IQuestionService questionService)
        {
            this.userManager = userManager;
            this.takeExamService = takeExamService;
            this.examService = examService;
            this.questionService = questionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTakeResult(string takeId)
        {
            try
            {
                var take = await this.takeExamService.GetTakeForView(takeId);

                return View("View", take);
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamNotFoundMessage;
                return Ok();
            }
        }

        public async Task<IActionResult> GetTakenExams(int p = 1, int s = 10)
        {
            var user = await this.userManager.GetUserAsync(User);
            var takes = await this.takeExamService.TakenExams(user.Id, p, s);

            return View("Taken", takes);
        }

        public async Task<IActionResult> GetUncompletedExams(int p = 1, int s = 10)
        {
            var user = await this.userManager.GetUserAsync(User);
            var exams = await this.takeExamService.UncompletedExams(user.Id, p, s);

            return View("Uncompleted", exams);
        }

        public IActionResult Confirm()
        {
            return PartialView("_ConfirmPartial");
        }

        public async Task<IActionResult> Start(string examId)
        {
            var user = await this.userManager.GetUserAsync(User);
            var takeExists = await this.takeExamService.TakeExists(user.Id, examId);

            if (takeExists)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = "Вече сте започнали да решавате този изпит!";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var exam = await this.examService.GetExamInfoAsync(examId);

                if (exam != null)
                {
                    return View("Start", exam);
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamNotActiveAnymoreMessage;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Continue(string takeId)
        {
            try
            {
                var takeExam = await this.takeExamService.GetTakeExamById(takeId);
                var questionOrder = this.questionService.GetLastNotTakenQuestionOrder(takeId);

                return RedirectToAction("GetNextQuestion", "Question", new
                {
                    takeId = takeId,
                    examId = takeExam.ExamId.ToString(),
                    order = questionOrder
                });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction(nameof(GetTakenExams));
            }
        }

        public async Task<IActionResult> Take(string examId)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(User);

                if (examId != null && user != null)
                {
                    var takeId = await this.takeExamService.CreateTake(user.Id, examId);

                    if (takeId != Guid.Empty)
                    {
                        int questionOrder = 0;
                        return RedirectToAction("GetNextQuestion", "Question", new { takeId = takeId, examId = examId, order = questionOrder });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }

        public async Task<IActionResult> Finish(string takeExamId)
        {
            try
            {
                if (await this.takeExamService.FinishExam(takeExamId))
                {
                    var take = await this.takeExamService.GetTakeForView(takeExamId);
                    return View("View", take);
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
