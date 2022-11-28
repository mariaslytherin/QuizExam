using QuizExam.Core.Models.Base;

namespace QuizExam.Core.Models.Exam
{
    public class ExamListVM : PageVM
    {
        public List<ViewExamVM> Exams { get; set; }
    }
}
