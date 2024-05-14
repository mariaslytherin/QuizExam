using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace QuizExam.Core.Validation
{
    public class TimeFormatAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string timeString = Convert.ToString(value);

            if (timeString == "00:00")
            {
                return new ValidationResult("Моля, въведете валидно времетраене.");
            }

            TimeSpan duration;
            var isValid = TimeSpan.TryParse(Convert.ToString(value), out duration);

            if (isValid)
            {
                if (duration.TotalSeconds > 0)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("Моля, въведете валидно времетраене.");
            }
            else
            {
                return new ValidationResult("Невалиден формат. Моля, въведете времетраене в 24-часов формат (ЧЧ:мм).");
            }
        }
    }
}
