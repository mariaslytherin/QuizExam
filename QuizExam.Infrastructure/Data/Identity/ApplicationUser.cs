using Microsoft.AspNetCore.Identity;
using QuizExam.Infrastructure.Data.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizExam.Infrastructure.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(UserValidationConstants.FirstNameMaxLength)]
        public string? FirstName { get; set; }

        [StringLength(UserValidationConstants.LastNameMaxLength)]
        public string? LastName { get; set; }

        public ICollection<TakeExam> Takes { get; set; } = new List<TakeExam>();
    }
}
