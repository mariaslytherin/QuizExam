namespace QuizExam.Core.Constants
{
    public static class SuccessMessageConstants
    {
        public const string SuccessMessage = "SuccessMessage";

        public const string SuccessfulEditMessage = "Успешна редакция!";

        public const string SuccessfulActivationMessage = "Успешно активиране!";

        public const string SuccessfulDeactivationMessage = "Успешно деактивиране!";

        public const string SuccessfulAddMessage = "Успешно добавяне!";

        public const string SuccessfulCreateMessage = "Успешно създаване!";

        public const string SuccessfulDeleteMessage = "Успешно изтриване!";

        public const string SuccessfulRecordMessage = "Успешен запис!";

        public const string SuccessfulAddedRoleMessage = "Успешно добавихте потребителя към роля!";
    }
    public static class ErrorMessageConstants
    {
        public const string ErrorMessage = "ErrorMessage";

        public const string ErrorAppeardMessage = "Възникна грешка!";

        public const string UnsuccessfulEditMessage = "Неуспешна редакция!";

        public const string UnsuccessfulCreateMessage = "Неуспешно създаване!";

        public const string UnsuccessfulDeleteMessage = "Неуспешно изтриване!";

        public const string UnsuccessfullAddMessage = "Неуспешно добавяне!";

        public const string UnsuccessfulActivationMessage = "Неуспешно активиране!";

        public const string UnsuccessfulDeactivationMessage = "Неуспешно активиране!";

        public const string ErrorMustCheckAnswerMessage = "Отбележете 1 верен отговор на въпроса!";

        public const string ErrorExamNotFoundMessage = "Не успяхме да намерим този изпит!";

        public const string ErrorExamNotActiveAnymoreMessage = "Изпитът вече не е активен!";

        public const string ErrorNotEnoughAnswerOptionsMessage = "Добавете поне 2 възможни опции за отговор!";
    }

    public static class WarningMessageConstants
    {
        public const string WarningMessage = "WarningMessage";

        public const string WarningCannotAddOptionMessage = "Един въпрос не може да има повече от 6 опции за отговор!";

        public const string WarningAddOptionsMessage = "Добавете възможни опции за отговор!";

        public const string WarningExamMissingQuestionsMessage = "За да активирате изпита трябва да добавите въпроси!";

        public const string WarningExamNotEqualPointsMessage = "За да активирате изпита сбора от точките на въпросите трябва да е равен на максималния брой точки на изпита!";
    }
}
