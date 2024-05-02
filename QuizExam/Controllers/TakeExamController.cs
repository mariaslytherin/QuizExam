using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Exam;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Enums;
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
                return RedirectToAction(nameof(GetTakenExams));
            }
        }

        public async Task<IActionResult> GetTakenExams(int p = 1, int s = 10)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(User);
                var takes = await this.takeExamService.TakenExams(user.Id, p, s);

                return View("Taken", takes);
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> GetUncompletedExams(int p = 1, int s = 10)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(User);
                var exams = await this.takeExamService.UncompletedExams(user.Id, p, s);

                return View("Uncompleted", exams);
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Start(string examId)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(User);
                var takeExists = await this.takeExamService.TakeExists(user.Id, examId);

                if (takeExists)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamAlreadyStartedMessage;
                    return RedirectToAction("Index", "Home");
                }

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
            catch
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

                return RedirectToAction("GetQuestion", "Question", new
                {
                    takeId,
                    examId = takeExam.ExamId.ToString(),
                    order = questionOrder,
                });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction(nameof(GetUncompletedExams));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Take(string examId, TakeExamModeEnum mode)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(User);

                if (examId != null && user != null)
                {
                    var takeId = await this.takeExamService.CreateTake(user.Id, examId, mode);

                    if (takeId != Guid.Empty)
                    {
                        int questionOrder = 0;
                        return RedirectToAction("GetQuestion", "Question", new
                        {
                            takeId,
                            examId,
                            order = questionOrder,
                        });
                    }
                }

                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Start", "TakeExam", new { examId });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamNotFoundMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Pause(string takeId, string timePassed)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(User);

                if (await this.takeExamService.PuaseExam(takeId, timePassed))
                {
                    var exams = await this.takeExamService.UncompletedExams(user.Id, page: 1, size: 10);

                    return View("Uncompleted", exams);
                }

                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamNotFoundMessage;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Finish(string takeId, string? timePassed = null)
        {
            try
            {
                if (await this.takeExamService.FinishExam(takeId, timePassed))
                {
                    var take = await this.takeExamService.GetTakeForView(takeId);
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
