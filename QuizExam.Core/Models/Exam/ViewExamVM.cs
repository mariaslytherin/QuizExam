﻿using QuizExam.Core.Models.Question;

namespace QuizExam.Core.Models.Exam
{
    public class ViewExamVM
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SubjectName { get; set; }

        public DateTime CreateDate { get; set; }

        public string IsActive { get; set; }

        public IList<QuestionExamVM> Questions { get; set; }
    }
}
