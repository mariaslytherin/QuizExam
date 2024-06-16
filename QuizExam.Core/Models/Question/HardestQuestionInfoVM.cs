using QuizExam.Core.Models.AnswerOption;

namespace QuizExam.Core.Models.Question
{
    public class HardestQuestionInfoVM
    {
        public string QuestionId { get; set; }

        public string Content { get; set; }

        public string Rule { get; set; }

        public double MistakePercentage { get; set; }

        public IList<AnswerOptionVM> AnswerOptions { get; set; }
    }
}
