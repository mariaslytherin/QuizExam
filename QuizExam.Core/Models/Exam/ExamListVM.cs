namespace QuizExam.Core.Models.Exam
{
    public class ExamListVM
    {
        public int? PageNo { get; set; }

        public int? PageSize { get; set; }

        public int TotalRecords { get; set; }

        public List<ViewExamVM> Exams { get; set; }
    }
}
