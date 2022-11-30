﻿using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.TakeAnswer;
using QuizExam.Core.Models.TakeQuestion;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Enums;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Core.Services
{
    public class TakeAnswerService : ITakeAnswerService
    {
        private readonly IApplicationDbRepository repository;

        public TakeAnswerService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> AddAnswer(TakeQuestionVM model, string examId)
        {
            try
            {
                var result = false;
                var takeExam = await this.repository.GetByIdAsync<TakeExam>(Guid.Parse(model.TakeExamId));

                if (takeExam != null)
                {
                    var answer = new TakeAnswer
                    {
                        TakeExamId = Guid.Parse(model.TakeExamId),
                        AnswerOptionId = Guid.Parse(model.CheckedOptionId),
                        QuestionId = Guid.Parse(model.QuestionId),
                    };
                    await this.repository.AddAsync(answer);
                    await this.repository.SaveChangesAsync();
                    result = true;
                }

                return result;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(TakeAnswer)}'. ");
            }
        }
    }
}