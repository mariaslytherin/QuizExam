using QuizExam.Core.Models.Base;

namespace QuizExam.Core.Models.TakeExam
{
    public class TakenExamsListVM : PageVM
    {
        public List<TakeExamVM> TakenExams { get; set; }
    }
}
