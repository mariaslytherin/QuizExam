﻿using QuizExam.Infrastructure.Data.Enums;
using QuizExam.Infrastructure.Data.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizExam.Infrastructure.Data
{
    public class TakeExam
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [Required]
        public Guid ExamId { get; set; }

        [ForeignKey(nameof(ExamId))]
        public Exam Exam { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime ModifyDate { get; set; } = DateTime.Now;

        public TakeExamStatusEnum Status { get; set; }

        public double Score { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? TimePassed { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? Duration { get; set; }

        public TakeExamModeEnum Mode { get; set; }

        public ICollection<TakeAnswer> TakeAnswers { get; set; } = new List<TakeAnswer>();
    }
}
