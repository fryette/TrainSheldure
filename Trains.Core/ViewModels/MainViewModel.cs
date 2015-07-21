using System;
using System.Collections.Generic;
using System.Linq;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Email;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Core.Services.Interfaces;
using Trains.Entities;
using Trains.Model.Entities;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using static System.String;

namespace Trains.Core.ViewModels
{
	public class MainViewModel : MvxViewModel
	{
		#region readonlyProperties

		private readonly IAppSettings _appSettings;

		private readonly IMvxComposeEmailTask _email;

		private readonly IMarketPlaceService _marketPlace;

		private readonly IAnalytics _analytics;

		private readonly ILocalDataService _local;


		private readonly IPattern _patterns;

		/// <summary>
		/// Used to serialization/deserialization objects.
		/// </summary>
		private readonly ISerializableService _serializable;


		/// <summary>
		/// Used to search train schedule.
		/// </summary>
		private readonly ISearchService _search;


		#endregion

		#region commands

		public MvxCommand<About> TappedAboutItemCommand { get; private set; }
		public IMvxCommand GoToHelpCommand { get; private set; }
		public MvxCommand<Train> SelectTrainCommand { get; private set; }
		public MvxCommand<Route> TappedFavoriteCommand { get; private set; }
		public IMvxCommand UpdateLastRequestCommand { get; private set; }
		public IMvxCommand SearchTrainCommand { get; private set; }
		public IMvxCommand SwapCommand { get; private set; }
		public MvxCommand<Route> TappedRouteCommand { get; private set; }

		#endregion

		#region ctor

		public MainViewModel(ISerializableService serializable, ISearchService search, IAppSettings appSettings, IMarketPlaceService marketPlace, IAnalytics analytics, IPattern pattern, ILocalDataService local, IMvxComposeEmailTask email)
		{
			_email = email;
			_local = local;
			_analytics = analytics;
			_serializable = serializable;
			_search = search;
			_appSettings = appSettings;
			_marketPlace = marketPlace;
			_patterns = pattern;

			TappedAboutItemCommand = new MvxCommand<About>(ClickAboutItem);
			GoToHelpCommand = new MvxCommand(GoToHelpPage);
			SelectTrainCommand = new MvxCommand<Train>(ClickItem);
			UpdateLastRequestCommand = new MvxCommand(UpdateLastRequest);
			SearchTrainCommand = new MvxCommand(() => SearchTrain(From.Trim(), To.Trim()));
			SwapCommand = new MvxCommand(Swap);
			TappedRouteCommand = new MvxCommand<Route>(SetRoute);
			TappedFavoriteCommand = new MvxCommand<Route>(route =>
			{
				if (route == null) return;
				SearchTrain(route.From, route.To);
			});
		}

		#endregion

		#region properties

		#region UIProperties

		public string ApplicationName { get; set; }
		public string MainPivotItem { get; set; }
		public string FastSearchTextBlock { get; set; }
		public string DateOfDeparture { get; set; }
		public string FromAutoSuggest { get; set; }
		public string ToAutoSuggest { get; set; }
		public string Search { get; set; }
		public string LastSchedulePivotItem { get; set; }
		public string RoutesPivotItem { get; set; }
		public string AboutPivotItem { get; set; }
		public string Update { get; set; }
		public string UpdateAppBar { get; set; }
		public string SwapAppBar { get; set; }
		public string ManageAppBar { get; set; }
		public string HelpAppBar { get; set; }
		public string LastRequests { get; set; }

		#endregion

		public IEnumerable<About> AboutItems { get; set; }

		private List<Route> _lastRoutes;
		public List<Route> LastRoutes
		{
			get
			{
				return _lastRoutes;
			}
			set
			{
				_lastRoutes = value;
				RaisePropertyChanged(() => LastRoutes);
			}
		}

		private DateTimeOffset _datum = new DateTimeOffset(DateTime.Now);
		public DateTimeOffset Datum
		{
			get { return _datum; }
			set
			{
				_datum = value;
				RaisePropertyChanged(() => Datum);
			}
		}

		/// <summary>
		/// Stores variant of search.
		/// </summary> 
		private List<string> _variantOfSearch;
		public List<string> VariantOfSearch
		{
			get
			{
				return _variantOfSearch;
			}
			set
			{
				_variantOfSearch = value;
				RaisePropertyChanged(() => VariantOfSearch);
			}
		}

		public string LastUpdateTime
		{
			get
			{
				if (_appSettings.UpdatedLastRequest == null) return null;
				var date = (DateTimeOffset.Now - _appSettings.UpdatedLastRequest.Date);
				return ResourceLoader.Instance.Resource["Updated"] + (date.TotalMinutes > 1 ? (date.Hours > 1 ? (date.Hours + ResourceLoader.Instance.Resource["Hour"]) :
					(date.Minutes + ResourceLoader.Instance.Resource["Min"])) + ResourceLoader.Instance.Resource["Ago"]
					: ResourceLoader.Instance.Resource["JustNow"]);
			}
		}

		/// <summary>
		/// Used to set code behind variant of search.
		/// </summary> 
		private string _selectedDate;
		public string SelectedVariant
		{
			get
			{
				return _selectedDate;
			}

			set
			{
				_selectedDate = value;
				RaisePropertyChanged(() => SelectedVariant);
			}
		}

		/// <summary>
		/// Contains stopping points satisfying user input.
		/// </summary> 
		private List<string> _autoSuggestions;
		public List<string> AutoSuggestions
		{
			get { return _autoSuggestions; }
			set
			{
				_autoSuggestions = value;
				RaisePropertyChanged(() => AutoSuggestions);
			}
		}

		/// <summary>
		/// Used to read and set start stop point.
		/// </summary> 
		private string _from;
		public string From
		{
			get { return _from; }
			set
			{
				_from = value;
				RaisePropertyChanged(() => From);
				UpdateAutoSuggestions(From);
			}
		}

		/// <summary>
		/// Used to read and set start end point.
		/// </summary> 
		private string _to;
		public string To
		{
			get { return _to; }
			set
			{
				_to = value;
				RaisePropertyChanged(() => To);
				UpdateAutoSuggestions(To);
			}
		}

		/// <summary>
		/// Used for process control.
		/// </summary>
		private bool _isTaskRun;
		public bool IsTaskRun
		{
			get { return _isTaskRun; }
			set
			{
				_isTaskRun = value;
				RaisePropertyChanged(() => IsTaskRun);
			}
		}

		/// <summary>
		/// Used for process control.
		/// </summary>
		private bool _isBarDownloaded;

		public bool IsBarDownloaded
		{
			get { return _isBarDownloaded; }
			set
			{
				_isBarDownloaded = value;
				RaisePropertyChanged(() => IsBarDownloaded);
			}
		}

		/// <summary>
		/// Used for process download data control.
		/// </summary>
		private bool _isDownloadRun;
		public bool IsDownloadRun
		{
			get { return _isDownloadRun; }
			set
			{
				_isDownloadRun = value;
				RaisePropertyChanged(() => IsDownloadRun);
			}
		}

		/// <summary>
		/// Keeps trains from the last request.
		/// </summary>
		private static List<Train> _trains;

		public List<Train> Trains
		{
			get { return _trains; }
			set
			{
				_trains = value;
				RaisePropertyChanged(() => Trains);
			}
		}

		/// <summary>
		/// Object are stored custom routes.
		/// </summary>
		private IEnumerable<Route> _favoriteRequests;
		public IEnumerable<Route> FavoriteRequests
		{
			get { return _favoriteRequests; }
			set
			{
				_favoriteRequests = value;
				RaisePropertyChanged(() => FavoriteRequests);
			}
		}

		/// <summary>
		/// Last route
		/// </summary>
		private string _lastRoute;
		public string LastRoute
		{
			get { return _lastRoute; }
			set
			{
				_lastRoute = value;
				RaisePropertyChanged(() => LastRoute);
			}
		}

		#endregion

		#region actions

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// Set the default parameter of some properties.
		/// </summary>
		public async void Init()
		{
			IsDownloadRun = true;
			if (!await RestoreData()) return;
			RestoreUiBinding();
			InitAboutItemsActions();
			IsBarDownloaded = true;
			if (_appSettings.UpdatedLastRequest != null)
				LastRoute = $"{_appSettings.UpdatedLastRequest.Route.From} - {_appSettings.UpdatedLastRequest.Route.To}";
			Trains = _appSettings.LastRequestTrain;
			LastRoutes = _appSettings.LastRoutes;
			FavoriteRequests = _appSettings.FavoriteRequests?.Select(x => x.Route);
			AboutItems = _appSettings.About;
			SelectedVariant = VariantOfSearch[1];
			IsDownloadRun = false;
		}

		/// <summary>
		/// Searches for train schedules at a specified date in the specified mode.
		/// </summary>
		private async void SearchTrain(string from, string to)
		{
			if (await CheckInput(Datum, from, to, _appSettings.AutoCompletion)) return;
			IsTaskRun = true;
			AddToLastRoutes(new Route { From = from, To = to });
			var schedule = await _search.GetTrainSchedule(_appSettings.AutoCompletion.First(x => x.Value == from),
					_appSettings.AutoCompletion.First(x => x.Value == to), Datum, SelectedVariant);
			if (schedule != null)
			{
				_appSettings.LastRequestTrain = schedule;
				_appSettings.UpdatedLastRequest = new LastRequest { Route = new Route { From = from, To = to }, SelectionMode = SelectedVariant, Date = Datum };
				_serializable.Serialize(_appSettings.UpdatedLastRequest, Constants.UpdateLastRequest);
				_serializable.Serialize(schedule, Constants.LastTrainList);
				ShowViewModel<ScheduleViewModel>(new { param = JsonConvert.SerializeObject(schedule) });

				_analytics.SentEvent(Constants.Analytics.VariantOfSearch, SelectedVariant);
			}

			IsTaskRun = false;
		}

		/// <summary>
		/// Update last route
		/// </summary>
		private async void UpdateLastRequest()
		{
			if (_appSettings.UpdatedLastRequest == null) return;
			IsTaskRun = true;

			var trains = await _search.GetTrainSchedule(_appSettings.AutoCompletion.First(x => x.Value == _appSettings.UpdatedLastRequest.Route.From),
				_appSettings.AutoCompletion.First(x => x.Value == _appSettings.UpdatedLastRequest.Route.To),
				_appSettings.UpdatedLastRequest.Date, _appSettings.UpdatedLastRequest.SelectionMode);

			if (trains == null)
				await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["InternetConnectionError"]);
			else
			{
				_appSettings.UpdatedLastRequest.Date = DateTimeOffset.Now;
				_serializable.Serialize(_appSettings.UpdatedLastRequest, Constants.UpdateLastRequest);
				RaisePropertyChanged(() => LastUpdateTime);
				Trains = trains;
				_serializable.Serialize(Trains, Constants.LastTrainList);
			}

			IsTaskRun = false;
		}

		/// <summary>
		/// Invoked when the user selects his train of interest.
		/// </summary>
		/// <param name="train">Data that describes user-selected train(prices,seats,stop points,and other)</param>
		private void ClickItem(Train train)
		{
			if (train == null) return;
			ShowViewModel<InformationViewModel>(new { param = JsonConvert.SerializeObject(train) });
		}

		/// <summary>
		/// Go to saved by user routes page.
		/// </summary>
		private void GoToHelpPage()
		{
			ShowViewModel<HelpViewModel>();
		}

		private void ClickAboutItem(About selectedAboutItem)
		{
			if (selectedAboutItem != null)
				AboutItemsActions[selectedAboutItem.Item]();
		}

		private Dictionary<AboutPicture, Action> AboutItemsActions { get; set; }

		/// <summary>
		/// Swaped From and To properties.
		/// </summary>
		private void Swap()
		{
			var tmp = From;
			From = To;
			To = tmp;
		}

		private void AddToLastRoutes(Route route)
		{
			var routes = new List<Route>() { route };
			if (LastRoutes == null) LastRoutes = new List<Route>();
			routes.AddRange(LastRoutes);
			_appSettings.LastRoutes = LastRoutes = routes.Take(3).GroupBy(x => new { x.From, x.To }).Select(g => g.First()).ToList();
			_serializable.Serialize(LastRoutes, Constants.LastRoutes);
		}

		private void SetRoute(Route route)
		{
			if (route == null) return;
			From = route.From;
			To = route.To;
		}

		/// <summary>
		/// Update prompts during user input stopping point
		/// </summary>
		public void UpdateAutoSuggestions(string str)
		{
			var station = str.Trim();
			if (IsNullOrEmpty(station)) AutoSuggestions = null;
			AutoSuggestions = _appSettings.AutoCompletion.Where(x => x.Value.IndexOf(station, StringComparison.OrdinalIgnoreCase) >= 0).Select(x => x.Value).ToList();
			if (AutoSuggestions.Count == 1 && AutoSuggestions[0] == station) AutoSuggestions = null;
		}

		public async Task<bool> CheckInput(DateTimeOffset datum, string from, string to, List<CountryStopPointItem> autoCompletion)
		{
			if ((datum.Date - DateTime.Now).Days < 0)
			{
				await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["DateUpTooLater"]);
				return true;
			}
			if (datum.Date > DateTime.Now.AddDays(45))
			{
				await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["DateTooBig"]);
				return true;
			}

			if (IsNullOrEmpty(from) || IsNullOrEmpty(to) ||
				!(autoCompletion.Any(x => x.Value == from.Trim()) &&
				  autoCompletion.Any(x => x.Value == to.Trim())))
			{
				await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["IncorrectInput"]);
				return true;
			}

			if (NetworkInterface.GetIsNetworkAvailable()) return false;
			await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["ConectionError"]);
			return true;
		}

		#region restoreResources


		private async Task<bool> RestoreData()
		{
			await CheckStart();
			if (_appSettings.AutoCompletion == null)
			{

				var appSettings = _serializable.Desserialize<AppSettings>(Constants.AppSettings);

				if (appSettings == null) return false;

				_appSettings.AutoCompletion = appSettings.AutoCompletion;
				_appSettings.About = appSettings.About;
				_appSettings.HelpInformation = appSettings.HelpInformation;
				_appSettings.CarriageModel = appSettings.CarriageModel;
				_appSettings.SocialUri = appSettings.SocialUri;
				_appSettings.Language = appSettings.Language;
				_appSettings.PlaceInformation = appSettings.PlaceInformation;
				_appSettings.Countries = appSettings.Countries;
				_appSettings.FavoriteRequests = _serializable.Desserialize<List<LastRequest>>(Constants.FavoriteRequests);
				_appSettings.UpdatedLastRequest = _serializable.Desserialize<LastRequest>(Constants.UpdateLastRequest);
				_appSettings.LastRequestTrain = _serializable.Desserialize<List<Train>>(Constants.LastTrainList);

				var routes = _serializable.Desserialize<List<Route>>(Constants.LastRoutes);
				_appSettings.LastRoutes = routes ?? new List<Route>();

				SetPatterns();
			}

			VariantOfSearch = new List<string>
				{
					ResourceLoader.Instance.Resource["Yesterday"],
					ResourceLoader.Instance.Resource["Today"],
					ResourceLoader.Instance.Resource["Tommorow"],
					ResourceLoader.Instance.Resource["OnAllDays"],
					ResourceLoader.Instance.Resource["OnDay"]
				};
			return true;
		}
		private async Task CheckStart()
		{
			var firstRun = _serializable.Desserialize<string>(Constants.IsFirstRun);
			if (firstRun == null)
			{
				_appSettings.Language = new Language { Id = "ru", Name = "Русский" };
				_serializable.ClearAll();
				_serializable.Serialize(Constants.IsFirstRun, Constants.IsFirstRun);
				_serializable.Serialize(_appSettings.Language, Constants.AppLanguage);
				await Mvx.Resolve<IUserInteraction>().AlertAsync(Constants.HiMessage,Constants.HiMessageTitle);
			}

			var appLanguage = _serializable.Desserialize<Language>(Constants.AppLanguage);
			var currLanguage = _serializable.Desserialize<Language>(Constants.CurrentLanguage);

			if (currLanguage == null || currLanguage.Id != appLanguage.Id)
			{
				try
				{
					await DowloadResources(currLanguage ?? appLanguage);
					DeleteSaveSettings();
				}

				catch (Exception)
				{
					await Mvx.Resolve<IUserInteraction>().AlertAsync("Произошла проблема с загрузкой ресурсов,проверьте доступ к интернету и повторите");
				}
			}
		}

		private void InitAboutItemsActions()
		{
			AboutItemsActions = new Dictionary<AboutPicture, Action>
			{
			{AboutPicture.AboutApp,()=>ShowViewModel<AboutViewModel>()},
			{AboutPicture.Mail,()=>_email.ComposeEmail("sampir.fiesta@gmail.com", Empty, ResourceLoader.Instance.Resource["Feedback"], Empty, false)},
			{AboutPicture.Market,()=>_marketPlace.GoToMarket()},
			{AboutPicture.Settings,()=>ShowViewModel<SettingsViewModel>()},
			{AboutPicture.Share,()=>ShowViewModel<ShareViewModel>()}
			};
		}

		private async Task DowloadResources(Language lang)
		{
			_appSettings.Language = lang;
			_appSettings.AutoCompletion = await _local.GetLanguageData<List<CountryStopPointItem>>(Constants.StopPointsJson);
			_appSettings.HelpInformation = await _local.GetLanguageData<List<HelpInformationItem>>(Constants.HelpInformationJson);
			_appSettings.CarriageModel = await _local.GetLanguageData<List<CarriageModel>>(Constants.CarriageModelJson);
			_appSettings.About = await _local.GetLanguageData<List<About>>(Constants.AboutJson);
			_appSettings.SocialUri = await _local.GetOtherData<SocialUri>(Constants.SocialJson);
			_appSettings.PlaceInformation = await _local.GetLanguageData<List<PlaceInformation>>(Constants.PlaceInformationJson);
			_appSettings.Countries = await _local.GetLanguageData<List<Country>>(Constants.Countries);

			_serializable.Serialize(await _local.GetLanguageData<Dictionary<string, string>>(Constants.ResourceJson), Constants.ResourceLoader);
			_serializable.Serialize(await _local.GetOtherData<Patterns>(Constants.PatternsJson), Constants.Patterns);
			_serializable.Serialize(_appSettings, Constants.AppSettings);
			_serializable.Serialize(_appSettings.Language, Constants.AppLanguage);
			_serializable.Serialize(_appSettings.Language, Constants.CurrentLanguage);
			SetPatterns();
		}

		private void SetPatterns()
		{
			var patterns = _serializable.Desserialize<Patterns>(Constants.Patterns);

			_patterns.AdditionParameterPattern = patterns.AdditionParameterPattern;
			_patterns.PlacesAndPricesPattern = patterns.PlacesAndPricesPattern;
			_patterns.TrainPointPAttern = patterns.TrainPointPAttern;
			_patterns.TrainsPattern = patterns.TrainsPattern;
		}

		private void DeleteSaveSettings()
		{
			_serializable.Delete(Constants.FavoriteRequests);
			_serializable.Delete(Constants.LastTrainList);
			_serializable.Delete(Constants.UpdateLastRequest);
		}

		private void RestoreUiBinding()
		{
			ApplicationName = ResourceLoader.Instance.Resource["ApplicationName"];
			MainPivotItem = ResourceLoader.Instance.Resource["MainPivotItem"];
			FastSearchTextBlock = ResourceLoader.Instance.Resource["FastSearchTextBlock"];
			DateOfDeparture = ResourceLoader.Instance.Resource["DateOfDeparture"];
			FromAutoSuggest = ResourceLoader.Instance.Resource["From"];
			ToAutoSuggest = ResourceLoader.Instance.Resource["To"];
			Search = ResourceLoader.Instance.Resource["Search"];
			LastSchedulePivotItem = ResourceLoader.Instance.Resource["LastSchedulePivotItem"];
			RoutesPivotItem = ResourceLoader.Instance.Resource["RoutesPivotItem"];
			AboutPivotItem = ResourceLoader.Instance.Resource["AboutPivotItem"];
			Update = ResourceLoader.Instance.Resource["Update"];
			UpdateAppBar = ResourceLoader.Instance.Resource["UpdateAppBar"];
			SwapAppBar = ResourceLoader.Instance.Resource["SwapAppBar"];
			ManageAppBar = ResourceLoader.Instance.Resource["ManageAppBar"];
			HelpAppBar = ResourceLoader.Instance.Resource["HelpAppBar"];
			LastRequests = ResourceLoader.Instance.Resource["LastRequests"];

			RaiseAllPropertiesChanged();
		}
		#endregion

		#endregion
	}
}
