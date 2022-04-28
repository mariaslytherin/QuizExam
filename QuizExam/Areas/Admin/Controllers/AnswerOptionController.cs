﻿using Microsoft.AspNetCore.Mvc;
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

        public AnswerOptionController(
            IAnswerOptionService answerService,
            IQuestionService questionService)
        {
            this.answerService = answerService;
            this.questionService = questionService;
        }

        public async Task<IActionResult> NewOption(string id, string examId)
        {
            var options = this.answerService.GetOptions(id).ToList();

            if (options.Count == 6)
            {
                TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningCannotAddOptionMessage;
                return RedirectToAction("Edit", "Question", new { id = id, examId = examId });
            }

            TempData["ExamId"] = examId;

            return View("QuestionAnswerOption");
        }

        [HttpPost]
        public async Task<IActionResult> NewOption(AddAnswerOptionVM model, string examId)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfullAddOptionMessage;
                return View("QuestionAnswerOption");
            }

            if (await this.answerService.Create(model))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedOptionMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = examId });
        }

        public async Task<IActionResult> SetCorrectAnswer(string id, string examId)
        {
            var options = this.answerService.GetOptions(id);
            var question = await this.questionService.GetQuestionById(id);
            TempData["ExamId"] = examId;
            TempData["QuestionContent"] = question.Content;

            var model = new SetCorrectAnswerVM
            {
                QuestionId = question.Id.ToString(),
                Options = options.ToList()
            };

            return View("SetCorrectAnswerOptions", model);
        }

        [HttpPost]
        public async Task<IActionResult> SetCorrectAnswer(SetCorrectAnswerVM model, string examId)
        {
            var checkedOptions = model.Options.Where(o => o.IsCorrect).ToList().Count;

            if (checkedOptions == 0)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckAnswerMessage;
                return RedirectToAction("SetCorrectAnswer", new { id = model.QuestionId, examId = examId });
            }
            else if(checkedOptions > 1)
            {
                TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.ErrorMustCheckOnlyOneMessage;
                return RedirectToAction("SetCorrectAnswer", new { id = model.QuestionId, examId = examId });
            }

            if (await this.answerService.SetCorrectAnswer(model))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedCorrectAnswerMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            TempData["ExamId"] = examId;

            return RedirectToAction("Edit", "Question", new { id = model.QuestionId, examId = examId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string questionId, string examId)
        {
            if (await this.answerService.Delete(id))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyDeletedOptionMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction("Edit", "Question", new { id = questionId, examId = examId });
        }
    }
}