using QuizExam.Core.Models.AnswerOption;

namespace QuizExam.Core.Models.Question
{
    public class QuestionExamVM
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public IList<AnswerOptionVM> AnswerOptions { get; set; }
    }
}
