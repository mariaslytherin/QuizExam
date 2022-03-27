using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizExam.Infrastructure.Data
{
    public class AnswerOption
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Content { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; } = DateTime.Today;

        [Column(TypeName = "date")]
        public DateTime ModifyDate { get; set; }

        public bool IsCorrect { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }
    }
}
