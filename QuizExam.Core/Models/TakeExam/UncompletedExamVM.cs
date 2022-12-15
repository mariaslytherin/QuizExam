namespace QuizExam.Core.Models.TakeExam
{
    public class UncompletedExamVM
    {
        public string TakeId { get; set; }

        public string Title { get; set; }

        public string SubjectName { get; set; }

        public string StartDate { get; set; }

        public int AllQuestionsCount { get; set; }

        public int TakenQuestionsCount { get; set; }

        public int Progress { get; set; }
    }
}
