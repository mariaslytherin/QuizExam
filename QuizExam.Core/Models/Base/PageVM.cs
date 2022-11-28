namespace QuizExam.Core.Models.Base
{
    public class PageVM
    {
        public int? PageNo { get; set; }

        public int? PageSize { get; set; }

        public int TotalRecords { get; set; }
    }
}
