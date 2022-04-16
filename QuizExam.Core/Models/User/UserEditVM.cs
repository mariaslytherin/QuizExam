using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models
{
    public class UserEditVM
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
    }
}
