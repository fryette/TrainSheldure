namespace Trains.Infrastructure
{
	public static class Defines
	{
		public class Common
		{
			public const string IsFirstRun = "IsFirstRun36";
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

			public static string TrainsPattern =
				"(?<startTime><div class=\"list_start\">([^<]*)<\\/?)|" +
				"(?<endTime><div class=\"list_end\">(.+?)</div>)|" +
				"(?<city><div class=\"list_text\">(.+?)<\\/?)|" +
				"(?<trainDescription><span class=\"list_text_small\">(.+?)<\\/?)|" +
				"<div class=\"train_type\">.+?>(?<type>[^<>]+)<\\/div>|" +
				"(?<internetRegistration><div class=\"b-legend\">(.+?)</div)";
			public static string PlacesAndPricesPattern =
				"(?<Name><td class=\"places_name\">([^<]+)</td>)|(?<quantity><td class=\"places_qty\">([^<]*)<)|(?<Price><td class=\"places_price\">([^<]*))";
			public static string AdditionParameterPattern = "div class=\"b-places\">(.*?)</div>";
			public static string TrainPointPattern =
				"(?<name>class=\"train_name -map train_text\">([^<]*)<\\/?)|(?<startTime>class=\"train_end-time\">(.+?)<\\/?)|(?<endTime>class=\"train_start-time\">(.+?)<\\/?)|(?<stopTime>class=\"train_stop-time\">(.+?)<\\/?)";

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
			public const string Tickets = "Tickets.json";
		}
		public class Analytics
		{
			public const string LanguageChanged = "LanguageChanged";
			public const string AddToFavorite = "AddToFavorite";
			public const string VariantOfSearch = "VariantOfSearch";
		}

		public class Ticket
		{
			public const string DateFormat = "dd.MM.yyyy";
			public const string Kodd = "textValue(kodd)";
			public const string TrainBack = "textValue(poezd_ii)";
			public const string TypeOfCarriageBack = "textValue(tip_vag_ii)";
			public const string NumberOfSeatsBack = "textValue(kol_m_ii)";
			public const string DepartureDateBack = "textValue(dat_o_ii)";
			public const string PhoneNumber = "textValue(tel)";
			public const string Hid = "textValue(hid)";
			public const string Delivery = "textValue(dostavka)";
			public const string NumberOfTickets = "textValue(kol_mest)";
			public const string DepartureDate = "textValue(dat_o)";
			public const string TrainNumber = "textValue(poezd)";
			public const string Email = "textValue(email)";
			public const string FullName = "textValue(f_zakaz)";
			public const string From = "textValue(nsto)";
			public const string To = "textValue(nstn)";
			public const string SendRequest = "send_z";
			public const string TypeOfCarriage = "textValue(tip_vag)";
			public const string DeliveryValue = "0";
			public const string HidValue = "-1";
			public const string Additional = "o_usl1";
			public const string CodePattern = @"(?<TicketNumber>textValue\(kodd\)"" value=""(.+?)"")";
			public const string EmailPattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
			public const string SendRequestValue = "Отправить заявку";
			public const string EmailError = "Проверьте правильность email";
			public const string FullNameError = "Проверьте ввод поля \"ФИО пассажира\"";
			public const string PhoneNumberError = "Проверьте правильность телефона";
			public const string DateError = "Заказывать можно, лишь за 3 дня до отправляния.";
			public const string ErrorRequest = "Произошла ошибка, убедитесь, что вы ввели все правильно, в случае повтора просим уведомить нас через обратную связь";
			public const string SuccsessfullyRequest = "Ваш заказ успешно принят. Ваш код для проверки на сайте: {0}";

		}
	}
}

