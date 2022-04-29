namespace QuizExam.Core.Models.AnswerOption
{
    public class SetCorrectAnswerVM
    {
        public string QuestionId { get; set; }

        public List<AnswerOptionVM> Options { get; set; }

        public string CorrectAnswerId { get; set; }
    }
}
