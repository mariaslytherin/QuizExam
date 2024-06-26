﻿using QuizExam.Core.Constants;
using QuizExam.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.Exam
{
    public class EditExamVM
    {
        public string Id { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [StringLength(150, ErrorMessage = ExamErrorMessages.ExamTitleLength, MinimumLength = 6)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = ExamErrorMessages.ExamDescriptionMaxLength, MinimumLength = 10)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Максимален брой точки")]
        public double? MaxScore { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [TimeFormat]
        [Display(Name = "Времетраене")]
        public string Duration { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [Display(Name = "Предмет")]
        public string SubjectName { get; set; }
    }
}
