﻿using QuizExam.Core.Models.TakeAnswer;

namespace QuizExam.Core.Contracts
{
    public interface ITakeAnswerService
    {
        Task<bool> AddAnswer(string takeId, TakeAnswerVM model);
    }
}
