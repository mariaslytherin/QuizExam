using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.AnswerOption
{
    public class QuestionAnswerOptionVM
    {
        public string QuestionId { get; set; }

        [Required]
        public string AnswerOption { get; set; }
    }
}
