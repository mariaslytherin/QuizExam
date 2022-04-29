using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Exam;
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

        public async Task<IActionResult> Start(string examId)
        {
            try
            {
                var exam = await this.examService.GetExamInfo(examId);

                if (exam != null)
                {
                    return View("StartTakingExam", exam);
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
                        return RedirectToAction("GetQuestion", "Question", new { takeId = takeId, examId = examId, order = questionOrder });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }
    }
}
