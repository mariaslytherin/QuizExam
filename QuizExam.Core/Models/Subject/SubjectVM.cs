using QuizExam.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Core.Models.Subject
{
    public class SubjectVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
