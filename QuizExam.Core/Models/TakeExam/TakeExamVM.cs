using QuizExam.Core.Models.Question;
using QuizExam.Infrastructure.Data.Enums;

namespace QuizExam.Core.Models.TakeExam
{
    public class TakeExamVM
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SubjectName { get; set; }

        public string CreateDate { get; set; }

        public TakeExamModeEnum Mode { get; set; }

        public string TimePassed { get; set; }

        public double ResultScore { get; set; }

        public double? MaxScore { get; set; }

        public IList<QuestionExamVM> Questions { get; set; }
    }
}
