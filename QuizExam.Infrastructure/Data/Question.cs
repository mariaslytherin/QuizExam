using QuizExam.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizExam.Infrastructure.Data
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(300)]
        public string Content { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; } = DateTime.Today;

        [Column(TypeName = "date")]
        public DateTime ModifyDate { get; set; }

        public bool IsDeleted { get; set; }

        public QuestionTypeEnum Type { get; set; }

        [StringLength(400)]
        public string? Rule { get; set; }

        [Required]
        public double Points { get; set; }

        [Required]
        public int OrderNumber { get; set; }

        [Range(2, 6)]
        public int AnswerOptionsCount { get; set; }

        [Required]
        public Guid ExamId { get; set; }

        [ForeignKey(nameof(ExamId))]
        public Exam Exam { get; set; }

        public ICollection<AnswerOption> Answers { get; set; } = new List<AnswerOption>();
    }
}
