using QuizExam.Core.Models.TakeAnswer;
using QuizExam.Infrastructure.Data.Enums;

namespace QuizExam.Core.Models.TakeQuestion
{
    public class TakeQuestionVM
    {
        public string QuestionId { get; set; }

        public string TakeExamId { get; set; }

        public string ExamId { get; set; }

        public string Content { get; set; }

        public int Order { get; set; }

        public string CheckedOptionId { get; set; }

        public bool IsLast { get; set; }

        public string Duration { get; set; }

        public string TimePassed { get; set; }

        public TakeExamModeEnum Mode { get; set; }

        public List<TakeAnswerVM> TakeAnswers { get; set; }
    }
}
