using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.Exam
{
    public class FilterExamsVM
    {
        [Display(Name = "Предмет")]
        public string SubjectId { get; set; }

        public List<ViewExamVM> Exams { get; set; }
    }
}
