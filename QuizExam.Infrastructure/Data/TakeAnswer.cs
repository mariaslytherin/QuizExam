using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizExam.Infrastructure.Data
{
    public class TakeAnswer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TakeExamId { get; set; }

        [ForeignKey(nameof(TakeExamId))]
        public TakeExam TakeExam { get; set; }

        [Required]
        public Guid AnswerOptionId { get; set; }

        [ForeignKey(nameof(AnswerOptionId))]
        public AnswerOption AnswerOption { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Column(TypeName = "date")]
        public DateTime ModifyDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
