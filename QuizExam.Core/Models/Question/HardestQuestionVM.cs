using QuizExam.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.Question
{
    public class HardestQuestionVM
    {
        [Display(Name = "Предмет")]
        public string SubjectId { get; set; }

        [Display(Name = "Изпит")]
        public string ExamId { get; set; }

        public List<HardestQuestionInfoVM> HardestQuestionsInfo { get; set; }
    }
}
