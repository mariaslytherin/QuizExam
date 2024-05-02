namespace QuizExam.Core.Models.AnswerOption
{
    public class SetCorrectAnswerVM
    {
        public SetCorrectAnswerVM()
        {
        }

        public SetCorrectAnswerVM(string questionId, string examId, List<AnswerOptionVM> options)
        {
            this.QuestionId = questionId;
            this.ExamId = examId;
            this.Options = options;
        }

        public string QuestionId { get; set; }

        public string ExamId { get; set; }

        public List<AnswerOptionVM> Options { get; set; }

        public string CorrectAnswerId { get; set; }
    }
}
