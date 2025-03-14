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

        public const string ErrorMustSelectRoleMessage = "Необходимо е да назначите поне една роля на потребителя!"; 

        public const string ErrorMustCheckAnswerMessage = "Необходимо е да изберете опция за отговор на въпроса!";

		public const string ErrorExamNotFoundMessage = "Не успяхме да намерим този изпит!";

		public const string ErrorExamNotActiveAnymoreMessage = "Изпитът вече не е активен!";

		public const string ErrorNotEnoughAnswerOptionsMessage = "Добавете поне 2 възможни опции за отговор!";

		public const string ErrorNotSelectedCorrectAnswerMessage = "Необходимо е да маркирате верен отговор!";

		public const string ErrorExamAlreadyStartedMessage = "Вече сте започнали да решавате този изпит!";

		public const string ErrorExamMustNotBeActive = "Не можете да добавяте въпроси, когато изпитът е активиран!";

        public const string ErrorExamMustBeDeactivatedToEdit = "Не можете да извършвате редакция по изпит, който е в статус \"Активен\"!";

		public const string ErrorQuesitonCannotAddOptionMessage = "Един въпрос не може да има повече от 6 опции за отговор!";
	}

	public static class WarningMessageConstants
	{
		public const string WarningMessage = "WarningMessage";

		public const string WarningAddOptionsMessage = "Добавете възможни опции за отговор!";

		public const string WarningExamMissingQuestionsMessage = "За да активирате изпита трябва да добавите въпроси!";

		public const string WarningExamNotEqualPointsMessage = "За да активирате изпита сбора от точките на въпросите трябва да е равен на максималния брой точки на изпита!";

        public const string WarningExamHasQuestionsWithoutCorrectAnswerMessage = "За да активирате изпита всички добавени отговори на въпросите трябва да имат по един верен отговор!";

        public const string WarningExamIsActiveMessage = "В момента изпита е в статус \"Активен\", всички извършени промени по него няма да бъдат запазени!";

        public const string WarningExamIsActiveEditQuestionMessage = "В момента изпита е в статус \"Активен\", всички извършени промени по въпросът няма да бъдат запазени!";
    }
}
