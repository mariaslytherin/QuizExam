using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Question;

namespace QuizExam.Areas.Admin.Controllers
{
    public class AnswerOptionController : BaseController
    {
        private readonly IAnswerOptionService answerService;
        private readonly IQuestionService questionService;
        private readonly IExamService examService;

        public AnswerOptionController(
            IAnswerOptionService answerService,
            IQuestionService questionService,
            IExamService examService)
        {
            this.answerService = answerService;
            this.questionService = questionService;
            this.examService = examService;
        }

        [HttpPost]
        public async Task<IActionResult> New(NewAnswerOptionVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfullAddMessage;
                    return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = model.ExamId });
                }

                var isExamDeactivated = await this.examService.IsExamDeactivatedAsync(model.ExamId);
                if (!isExamDeactivated)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamMustBeDeactivatedToEdit;
                    return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = model.ExamId });
                }

                if (!await this.answerService.HasLessThenSixOptionsAsync(model.QuestionId))
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorQuesitonCannotAddOptionMessage;
                    return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = model.ExamId });
                }

                if (await this.answerService.CreateAsync(model))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulAddMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfullAddMessage;
                }

                return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = model.ExamId });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfullAddMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = model.ExamId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetCorrectAnswer(SetCorrectAnswerVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckAnswerMessage;
                    return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = model.ExamId });
                }

                var isExamDeactivated = await this.examService.IsExamDeactivatedAsync(model.ExamId);
                if (!isExamDeactivated)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamMustBeDeactivatedToEdit;
                    return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = model.ExamId });
                }

                if (model.CorrectAnswerId == null)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckAnswerMessage;
                    return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = model.ExamId });
                }

                if (await this.answerService.SetCorrectAnswerAsync(model))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulRecordMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                }

                return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = model.ExamId });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = model.ExamId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string questionId, string examId)
        {
            try
            {
                var isExamDeactivated = await this.examService.IsExamDeactivatedAsync(examId);
                if (!isExamDeactivated)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorExamMustBeDeactivatedToEdit;
                    return RedirectToAction("Edit", "Question", new { id = questionId, examId = examId });
                }

                if (await this.answerService.DeleteAsync(id))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulDeleteMessage;
                }
                else
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeleteMessage;
                }

                return RedirectToAction("Edit", "Question", new { id = questionId, examId });
            }
            catch
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulDeleteMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }
    }
}
