﻿using QuizExam.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Infrastructure.Data
{
    public class Subject
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(SubjectValidationConstants.NameMaxLength)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}
