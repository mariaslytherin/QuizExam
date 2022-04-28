using System.ComponentModel.DataAnnotations;

namespace QuizExam.Infrastructure.Data
{
    public class TakeExam
    {
        [Required]
        public Guid Id { get; set; }
    }
}
