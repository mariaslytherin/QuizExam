using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.Exam
{
    public class ExamVM
    {
        [Required]
        [StringLength(150)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        public int? MaxScore { get; set; }
    }
}
