namespace QuizExam.Core.Models.Exam
{
    public class ExamVM
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SubjectName { get; set; }

        public int QuestionsCount { get; set; }

        public string Duration { get; set; }
    }
}
