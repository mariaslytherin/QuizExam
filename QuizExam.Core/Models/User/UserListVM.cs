namespace QuizExam.Core.Models
{
    public class UserListVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
