﻿namespace QuizExam.Core.Models.Exam
{
    public class ExamListVM
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string SubjectName { get; set; }

        public DateTime CreateDate { get; set; }

        public string IsActive { get; set; }
    }
}