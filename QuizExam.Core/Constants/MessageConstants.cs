namespace QuizExam.Core.Constants
{
    public static class SuccessMessageConstants
    {
        public const string SuccessMessage = "SuccessMessage";

        public const string SuccessfulEditMessage = "Успешна редакция!";

        public const string SuccessfulActivationMessage = "Успешно активиране!";

        public const string SuccessfulDeactivationMessage = "Успешно деактивиране!";

        public const string SuccessfulDeletionMessage = "Успешно изтриване!";

        public const string SuccessfulAddedRoleMessage = "Успешно добавихте потребителя към роля!";

        public const string SuccessfullyAddedSubjectMessage = "Успешно добавихте предмет!";

        public const string SuccessfullyDeletedSubjectMessage = "Успешно изтрихте предмет!";

        public const string SuccessfullyAddedExamMessage = "Успешно създадохте изпит!";

        public const string SuccessfullyAddedQuestionMessage = "Успешно създадохте въпрос!";

        public const string SuccessfullyAddedOptionMessage = "Успешно добавихте опция!";

        public const string SuccessfullyDeletedQuestionMessage = "Успешно изтрихте въпрос!";

        public const string SuccessfullyDeletedOptionMessage = "Успешно изтрихте опция за отговор!";

        public const string SuccessfullyAddedCorrectAnswerMessage = "Успешно записахте верен отговор!";
    }
    public static class ErrorMessageConstants
    {
        public const string ErrorMessage = "ErrorMessage";

        public const string ErrorAppeardMessage = "Възникна грешка!";

        public const string UnsuccessfulEdit = "Неуспешна редакция!";

        public const string UnsuccessfulDeletionMessage = "Неуспешно изтриване!";

        public const string UnsuccessfulExamCreationMessage = "Не успяхме да създадем този изпит!";

        public const string UnsuccessfulAddQuestionMessage = "Неуспешно създаване на въпрос!";

        public const string UnsuccessfullAddOptionMessage = "Неуспешно добавяне на опция!";

        public const string ErrorMustCheckAnswerMessage = "Отбележете 1 верен отговор на въпроса!";

        public const string ErrorMustCheckOnlyOneMessage = "Въпросът може да има само 1 верен отговор!";

        public const string ErrorExamNotFoundMessage = "Не успяхме да намерим този изпит!";

        public const string ErrorExamNotActiveAnymoreMessage = "Изпитът вече не е активен!";
    }

    public static class WarningMessageConstants
    {
        public const string WarningMessage = "WarningMessage";

        public const string WarningCannotAddOptionMessage = "Един въпрос не може да има повече от 6 опции за отговор!";

        public const string WarningAddOptionsMessage = "Добавете възможни опции за отговор!";

        public const string WarningActivationMessage = "За да активирате изпита трябва да добавите въпроси!";
    }
}
