using QuizExam.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.AnswerOption
{
    public class NewAnswerOptionVM
    {
        public string QuestionId { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [Display(Name = "Опция")]
        public string Content { get; set; }
    }
}
