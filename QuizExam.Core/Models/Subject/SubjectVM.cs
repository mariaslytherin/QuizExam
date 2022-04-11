using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.Subject
{
    public class SubjectVM
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Enter name of a subject")]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
