namespace QuizExam.Core.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the value of the current DateTime object to its equivalent string representation using the current specified format (dd/MM/yyyy).
        /// </summary>
        /// <param name="date"></param>
        /// <returns>A string representation of value of the current DateTime object.</returns>
        public static string ToDateOnlyString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
    }
}
