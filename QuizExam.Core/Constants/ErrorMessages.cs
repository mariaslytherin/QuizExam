﻿namespace QuizExam.Core.Constants
{
    public class GlobalErrorMessages
    {
        public const string FieldRequired = "Полето {0} е задължително.";
    }
    public class UserErrorMessages
    {
        public const string NameMaxMinLength = "Полето {0} трябва да бъде между {2} и {1} символа.";
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
        public const string ExamTitleMaxLength = "Заглавието трябва да бъде между {2} и {1} символа.";
        public const string ExamDescriptionMaxLength = "Описанието трябва да бъде между {2} и {1} символа.";
    }
}