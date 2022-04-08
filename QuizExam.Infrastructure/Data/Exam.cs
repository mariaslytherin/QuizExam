using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizExam.Infrastructure.Data
{
    public class Exam
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime CreateDate { get; set; } = DateTime.Today;

        [Column(TypeName = "date")]
        public DateTime ModifyDate { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public int? QuestionsCount { get; set; }

        public int? MaxScore { get; set; }

        [Required]
        public Guid SubjectId { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
