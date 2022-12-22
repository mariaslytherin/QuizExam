namespace QuizExam.Core.Extensions
{
    public static class GuidExtension
    {
        /// <summary>
        /// Converts the string representation of a GUID to the equivalent Guid structure.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>A new System.Guid structure that contains the value that was parsed.</returns>
        public static Guid ToGuid(this string guid)
        {
            return String.IsNullOrEmpty(guid) ? Guid.Empty : Guid.Parse(guid.ToLower());
        }
    }
}
