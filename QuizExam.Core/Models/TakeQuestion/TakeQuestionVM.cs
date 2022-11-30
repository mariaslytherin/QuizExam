﻿using QuizExam.Core.Models.TakeAnswer;

namespace QuizExam.Core.Models.TakeQuestion
{
    public class TakeQuestionVM
    {
        public string QuestionId { get; set; }

        public string TakeExamId { get; set; }

        public string ExamId { get; set; }

        public string Content { get; set; }

        public int Order { get; set; }

        public string CheckedOptionId { get; set; }

        public bool IsLast { get; set; }

        public List<TakeAnswerVM> TakeAnswers { get; set; }
    }
}