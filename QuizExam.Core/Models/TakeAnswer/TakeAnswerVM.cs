namespace QuizExam.Core.Models.TakeAnswer
{
    public class TakeAnswerVM
    {
        public string AnswerId { get; set; }

        public string OptionId { get; set; }

        public string Content { get; set; }

        public bool IsChecked { get; set; }
    }
}
