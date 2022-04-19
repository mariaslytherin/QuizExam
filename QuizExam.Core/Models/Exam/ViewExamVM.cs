namespace QuizExam.Core.Models.Exam
{
    public class ViewExamVM
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SubjectName { get; set; }

        public IList<string> Questions { get; set; }
    }
}
