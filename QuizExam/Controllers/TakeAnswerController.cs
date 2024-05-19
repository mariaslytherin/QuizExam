﻿using Microsoft.AspNetCore.Mvc;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.TakeQuestion;

namespace QuizExam.Controllers
{
    public class TakeAnswerController : Controller
    {
        private readonly ITakeAnswerService takeAnswerService;

        public TakeAnswerController(
            ITakeAnswerService takeAnswerService)
        {
            this.takeAnswerService = takeAnswerService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TakeQuestionVM model)
        {
            try
            {
                if (model.CheckedOptionId == null)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckAnswerMessage;
                    return Ok(new { errorMessage = ErrorMessageConstants.ErrorMustCheckAnswerMessage });
                }

                if (await this.takeAnswerService.AddAnswer(model, model.ExamId))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulRecordMessage;
                }
                else
                {
                    return Ok(new { errorMessage = ErrorMessageConstants.ErrorAppeardMessage });
                }

                TempData["ExamId"] = model.ExamId;

                if (model.IsLast)
                {
                    return Ok(new { isFinished = true, takeExamId = model.TakeExamId });
                }
                else
                {
                    return RedirectToAction("GetNextQuestion", "Question", new { takeId = model.TakeExamId, model.ExamId, order = model.Order + 1 });
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
