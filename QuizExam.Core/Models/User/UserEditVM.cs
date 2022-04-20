using QuizExam.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models
{
    public class UserEditVM
    {
        public string Id { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [StringLength(50, ErrorMessage = UserErrorMessages.NameMaxMinLength, MinimumLength = 2)]
        [Display(Name = "Име")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [StringLength(80, ErrorMessage = UserErrorMessages.NameMaxMinLength, MinimumLength = 2)]
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }
    }
}
