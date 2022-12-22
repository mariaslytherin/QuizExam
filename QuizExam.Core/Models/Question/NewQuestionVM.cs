using QuizExam.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.Question
{
    public class NewQuestionVM
    {
        public string ExamId { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [StringLength(300, ErrorMessage = QuestionErrorMessages.QuestionContentLength, MinimumLength = 15)]
        [Display(Name = "Въпрос")]
        public string Content { get; set; }

        [StringLength(400, ErrorMessage = QuestionErrorMessages.QuestionRuleLength, MinimumLength = 15)]
        [Display(Name = "Правило")]
        public string Rule { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [Display(Name = "Брой точки")]
        public double Points { get; set; }
    }
}
