namespace QuizExam.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateOnlyString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
    }
}
