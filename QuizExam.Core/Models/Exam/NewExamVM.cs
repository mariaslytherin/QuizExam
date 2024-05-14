using QuizExam.Core.Constants;
using QuizExam.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.Exam
{
    public class NewExamVM
    {
        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [StringLength(150, ErrorMessage = ExamErrorMessages.ExamTitleLength, MinimumLength = 6)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = ExamErrorMessages.ExamDescriptionMaxLength)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Максимален брой точки")]
        public int? MaxScore { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [TimeFormat]
        [Display(Name = "Времетраене")]
        public string Duration { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [Display(Name = "Предмет")]
        public string SubjectId { get; set; }
    }
}
