namespace Trains.Core.Resources
{
	public static class Defines
	{
		public class Common
		{
			public const string IsFirstRun = "IsFirstRun35";
			public const string DateFormat = "yy-MM-dd";
			public const string TimeFormat = "yy-MM-dd HH:mm";
			public const string HiMessageTitle = "Обязательно к прочтению";
			public const string HiMessage =
				"Мы предупреждаем, все данные берутся с официального сайта rw.by, мы просим не вымещать на нас злость, и в случае ошибки не минусовать, а отправить нам сообщение через приложение.\r\n" +
				"Мы убедительно просим вас проголосовать в магазине приложений, это дает нам стимул работать дальше.\r\n" +
				"Все свои пожелания можете отправлять через приложения и мы реализуем их в следующих обновлениях.\r\n" +
				"Спасибо за то, что вы с нами ;)";
			public const int NumberOfBelarussianStopPoints = 956;
			public static string Name = "Chygunka";
		}

		public class Uri
		{
			public const string LanguagesUri = "http://chygunka.by/languages/";
			public const string PatternsUri = "http://chygunka.by/otherData/";
			public const string CountriesFolder = "Countries/";
		}
		public class Restoring
		{
			public const string Social = "SocialUri";
			public const string AppSettings = "AppSettings";
			public const string Patterns = "Patterns";
			public const string ResourceLoader = "ResourceLoader";
			public const string UpdateLastRequest = "updateLastRequst";
			public const string FavoriteRequests = "favoriteRequests";
			public const string LastTrainList = "LastTrainList";
			public const string CurrentLanguage = "CurrentLanguage";
			public const string AppLanguage = "AppLanguage";
			public const string LastRoutes = "LastRoutes";
		}
		public class DownloadJson
		{
			public const string PlaceInformation = "PlaceInformation.json";
			public const string Social = "SocialUri.json";
			public const string Patterns = "Patterns.json";
			public const string StopPoints = "Countries/Belarus.json";
			public const string Countries = "Countries.json";
			public const string HelpInformation = "HelpInformation.json";
			public const string CarriageModel = "CarriageModel.json";
			public const string About = "About.json";
			public const string Resource = "Resource.json";
		}
		public class Analytics
		{
			public const string LanguageChanged = "LanguageChanged";
			public const string AddToFavorite = "AddToFavorite";
			public const string VariantOfSearch = "VariantOfSearch";
		}
	}
}

