namespace QuizExam.Core.Constants
{
    public class GlobalErrorMessages
    {
        public const string FieldRequired = "Полето {0} е задължително.";
    }
    public class UserErrorMessages
    {
		public const string NameMaxMinLength = "Полето {0} трябва да съдържа между {2} и {1} символа.";
		public const string InvalidEmail = "Полето {0} не е валиден имейл адрес.";
        public const string MaxStringLength = "Полето {0} може да съдържа максимум {1} знака.";
    }
    public class ExamErrorMessages
    {
        public const string ExamTitleLength = "Заглавието трябва да бъде между {2} и {1} символа.";
        public const string ExamDescriptionMaxLength = "Описанието трябва да бъде между най-много {1} символа.";
    }
    public class SubjectErrorMessages
    {
        public const string SubjectNameMaxLength = "Името трябва да бъде между {2} и {1} символа.";
    }
    public class QuestionErrorMessages
    {
        public const string QuestionContentLength = "Въпросът трябва да бъде между {2} и {1} символа.";
        public const string QuestionRuleLength = "Правилото трябва да бъде между {2} и {1} символа.";
    }
}
