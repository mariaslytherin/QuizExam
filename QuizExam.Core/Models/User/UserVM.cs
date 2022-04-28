namespace QuizExam.Core.Models.User
{
    public class UserVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
