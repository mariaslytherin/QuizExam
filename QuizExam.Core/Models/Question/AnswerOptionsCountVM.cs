using QuizExam.Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.NewFolder
{
    public class AnswerOptionsCountVM
    {
        [Required]
        [Display(Name = "Брой възможни опции")]
        public string Count { get; set; }

        [Required]
        [Display(Name = "Тип на въпроса")]
        public QuestionTypeEnum Type { get; set; }
    }
}
