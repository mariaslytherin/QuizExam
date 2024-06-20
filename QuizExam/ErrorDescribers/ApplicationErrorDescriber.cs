using Microsoft.AspNetCore.Identity;

namespace QuizExam.ErrorDescribers
{
    public class ApplicationErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            var error = base.DuplicateUserName(userName);
            error.Description = "В системата вече има регистриран потребител с този имейл адрес.";
            return error;
        }

        public override IdentityError DuplicateEmail(string email)
        {
            var error = base.DuplicateEmail(email);
            error.Description = "В системата вече има регистриран потребител с този имейл адрес.";
            return error;
        }

        public override IdentityError PasswordMismatch()
        {
            var error = base.PasswordMismatch();
            error.Description = "Неправилна парола.";
            return error;
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            var error = base.PasswordRequiresNonAlphanumeric();
            error.Description = "Паролата трябва да съдържа поне един символ различен от буква или цифра.";
            return error;
        }

        public override IdentityError PasswordRequiresLower()
        {
            var error = base.PasswordRequiresLower();
            error.Description = "Паролата трябва да съдържа поне една малка буква.";
            return error;
        }

        public override IdentityError PasswordRequiresUpper()
        {
            var error = base.PasswordRequiresLower();
            error.Description = "Паролата трябва да съдържа поне една главна буква.";
            return error;
        }

        public override IdentityError PasswordRequiresDigit()
        {
            var error = base.PasswordRequiresLower();
            error.Description = "Паролата трябва да съдържа поне една цифра.";
            return error;
        }

        public override IdentityError PasswordTooShort(int length)
        {
            var error = base.PasswordRequiresLower();
            error.Description = "Паролата трябва да съдържа поне 6 символа.";
            return error;
        }
    }
}
