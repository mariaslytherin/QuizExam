using QuizExam.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.Subject
{
    public class NewSubjectVM
    {
        public string Id { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [StringLength(100, ErrorMessage = SubjectErrorMessages.SubjectNameMaxLength, MinimumLength = 2)]
        [Display(Name = "Име на предмет")]
        public string Name { get; set; }
    }
}
