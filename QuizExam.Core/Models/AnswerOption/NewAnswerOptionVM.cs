using QuizExam.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.AnswerOption
{
    public class NewAnswerOptionVM
    {
        public NewAnswerOptionVM()
        {
        }

        public NewAnswerOptionVM(string questionId, string examId)
        {
            this.QuestionId = questionId;
            this.ExamId = examId;
        }

        public string QuestionId { get; set; }

        public string ExamId { get; set; }

        [Required(ErrorMessage = GlobalErrorMessages.FieldRequired)]
        [Display(Name = "Опция")]
        public string OptionContent { get; set; }
    }
}
