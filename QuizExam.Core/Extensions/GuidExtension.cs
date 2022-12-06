namespace QuizExam.Core.Extensions
{
    public static class GuidExtension
    {
        public static Guid ToGuid(this string guid)
        {
            return Guid.Parse(guid.ToLower());
        }
    }
}
