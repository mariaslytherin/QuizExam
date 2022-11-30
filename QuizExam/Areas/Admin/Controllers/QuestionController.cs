﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuizExam.Core.Constants;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Question;
using QuizExam.Infrastructure.Data.Enums;

namespace QuizExam.Areas.Admin.Controllers
{
    public class QuestionController : BaseController
    {
        private readonly IQuestionService questionService;
        private readonly IAnswerOptionService answerOptionService;

        public QuestionController(IQuestionService questionService, IAnswerOptionService answerOptionService)
        {
            this.questionService = questionService;
            this.answerOptionService = answerOptionService;
        }

        public IActionResult New()
        {
            return View("New");
        }

        [HttpPost]
        public async Task<IActionResult> New(NewQuestionVM model, string examId)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessageConstants.UnsuccessfulAddQuestionMessage] = ErrorMessageConstants.UnsuccessfulAddQuestionMessage;
                return View(model);
            }

            var questionId = await this.questionService.Create(model);

            if (questionId != Guid.Empty)
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyAddedQuestionMessage;
                TempData[WarningMessageConstants.WarningMessage] = WarningMessageConstants.WarningAddOptionsMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction("Edit", new { id = questionId, examId = examId });
        }

        public async Task<IActionResult> Edit(string examId, string id)
        {
            try
            {
                var question = await this.questionService.GetQuestionForEdit(id);
                var options = this.answerOptionService.GetOptions(id);

                if (question != null && options != Enumerable.Empty<AnswerOptionVM>())
                {
                    TempData["ExamId"] = examId;
                    
                    if (options.Count() != 0)
                    {
                        ViewBag.Options = options;
                    }

                    return View("Edit", question);
                }
                else
                {
                    TempData[ErrorMessageConstants.UnsuccessfulAddQuestionMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                    return RedirectToAction("ViewExam", "Exam", new { id = examId });
                }
            }
            catch
            {
                TempData[ErrorMessageConstants.UnsuccessfulAddQuestionMessage] = ErrorMessageConstants.ErrorAppeardMessage;
                return RedirectToAction("ViewExam", "Exam", new { id = examId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditQuestionVM model, string examId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[ErrorMessageConstants.ErrorMessage] = ErrorMessageConstants.UnsuccessfulEdit;
                    return View(model);
                }

                if (await this.questionService.Edit(model))
                {
                    TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfulEditMessage;
                }
                else
                {
                    throw new Exception("An error appeard!");
                }
            }
            catch (Exception)
            {

                throw;
            }
            

            return RedirectToAction("ViewExam", "Exam", new { id = examId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string examId)
        {
            if (await this.questionService.Delete(id))
            {
                TempData[SuccessMessageConstants.SuccessMessage] = SuccessMessageConstants.SuccessfullyDeletedQuestionMessage;
            }
            else
            {
                throw new Exception("An error appeard!");
            }

            return RedirectToAction("ViewExam", "Exam", new { id = examId });
        }
    }
}
