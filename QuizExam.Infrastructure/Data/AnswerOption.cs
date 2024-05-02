using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizExam.Infrastructure.Data
{
    public class AnswerOption
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(300)]
        public string Content { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime")]
        public DateTime ModifyDate { get; set; } = DateTime.Now;

        public bool IsCorrect { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }

        public ICollection<TakeAnswer> TakeAnswers { get; set; } = new List<TakeAnswer>();
    }
}
