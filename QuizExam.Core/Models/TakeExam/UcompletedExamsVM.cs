using QuizExam.Core.Models.Base;

namespace QuizExam.Core.Models.TakeExam
{
    public class UncompletedExamsVM : PageVM
    {
        public List<UncompletedExamVM> UncompletedExams { get; set; }
    }
}
