using QuizExam.Core.Models.User;

namespace QuizExam.Core.Models
{
    public class UserListVM
    {
        public int PageNo { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public List<UserVM> Users { get; set; }
    }
}
