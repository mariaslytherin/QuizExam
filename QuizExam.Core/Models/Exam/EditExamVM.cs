using QuizExam.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.Exam
{
    public class EditExamVM
    {
        public string Id { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [StringLength(150, ErrorMessage = ExamErrorMessages.ExamTitleLength, MinimumLength = 6)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = ExamErrorMessages.ExamDescriptionMaxLength, MinimumLength = 10)]
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Maximum score")]
        public int? MaxScore { get; set; }

        public string SubjectName { get; set; }
    }
}
