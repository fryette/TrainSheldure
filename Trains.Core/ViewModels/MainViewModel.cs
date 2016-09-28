using System;
using System.Collections.Generic;
using System.Linq;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.MvvmCross.Plugins.Email;
using Cirrious.MvvmCross.ViewModels;
using Trains.Model.Entities;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Infrastructure.Interfaces.Services;
using Trains.Model;

namespace Trains.Core.ViewModels
{
	public class MainViewModel : MvxViewModel
	{
		private readonly IAppSettings _appSettings;
		private readonly IMvxComposeEmailTask _email;
		private readonly IMarketPlaceService _marketPlace;
		private readonly IAnalytics _analytics;
		private readonly ISorageProvider _sorage;
		private readonly ISearchService _search;
		private readonly INotificationService _notificationService;
		private readonly IUserInteraction _userInteraction;
		private readonly ILocalizationService _localizationService;
		private readonly IJsonConverter _jsonConverter;

		public IMvxCommand GoToHelpCommand { get; private set; }
		public IMvxCommand UpdateLastRequestCommand { get; private set; }
		public IMvxCommand SearchTrainCommand { get; private set; }
		public IMvxCommand SwapCommand { get; private set; }
		public MvxCommand<TrainModel> SelectTrainCommand { get; private set; }
		public MvxCommand<About> TappedAboutItemCommand { get; private set; }
		public MvxCommand<Route> TappedRouteCommand { get; private set; }
		public MvxCommand<TrainModel> NotifyAboutSelectedTrainCommand { get; private set; }

		private List<string> _variantOfSearch;
		private List<string> _autoSuggestions;
		private IEnumerable<TrainModel> _trains;
		private List<Route> _lastRoutes;
		private DateTimeOffset _searchDate;
		private string _selectedDate;
		private string _to;
		private string _from;
		private Dictionary<AboutPicture, Action> AboutItemsActions { get; set; }
		private bool _isLoading;
		public IEnumerable<About> AboutItems { get; set; }

		public List<Route> LastRoutes
		{
			get { return _lastRoutes; }
			set
			{
				_lastRoutes = value;
				RaisePropertyChanged(() => LastRoutes);
			}
		}

		public DateTimeOffset SearchDate
		{
			get { return _searchDate; }
			set
			{
				_searchDate = value;
				RaisePropertyChanged(() => SearchDate);
			}
		}

		public List<string> VariantOfSearch
		{
			get { return _variantOfSearch; }
			set
			{
				_variantOfSearch = value;
				RaisePropertyChanged(() => VariantOfSearch);
			}
		}

		public TimeSpan LastUpdateTime => DateTimeOffset.Now - _appSettings.UpdatedLastRequest.Date;

		public string SelectedVariant
		{
			get { return _selectedDate; }

			set
			{
				_selectedDate = value;
				RaisePropertyChanged(() => SelectedVariant);
			}
		}

		public List<string> AutoSuggestions
		{
			get { return _autoSuggestions; }
			set
			{
				_autoSuggestions = value;
				RaisePropertyChanged(() => AutoSuggestions);
			}
		}

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

		public bool IsLoading
		{
			get { return _isLoading; }
			set
			{
				_isLoading = value;
				RaisePropertyChanged(() => IsLoading);
			}
		}

		public IEnumerable<TrainModel> Trains
		{
			get { return _trains; }
			set
			{
				_trains = value;
				RaisePropertyChanged(() => Trains);
			}
		}

		public MainViewModel(
			ISorageProvider sorage,
			ISearchService search,
			IAppSettings appSettings,
			IMarketPlaceService marketPlace,
			IAnalytics analytics,
			IMvxComposeEmailTask email,
			INotificationService notificationService,
			IUserInteraction userInteraction,
			ILocalizationService localizationService,
			IJsonConverter jsonConverter)
		{
			_email = email;
			_analytics = analytics;
			_sorage = sorage;
			_search = search;
			_appSettings = appSettings;
			_marketPlace = marketPlace;
			_notificationService = notificationService;
			_userInteraction = userInteraction;
			_localizationService = localizationService;
			_jsonConverter = jsonConverter;

			TappedAboutItemCommand = new MvxCommand<About>(ClickAboutItem);
			GoToHelpCommand = new MvxCommand(GoToHelpPage);
			SelectTrainCommand = new MvxCommand<TrainModel>(ClickItem);
			UpdateLastRequestCommand = new MvxCommand(UpdateLastRequest);
			SearchTrainCommand = new MvxCommand(async () => await SearchTrainAsync(From?.Trim(), To?.Trim()));
			NotifyAboutSelectedTrainCommand = new MvxCommand<TrainModel>(NotifyAboutSelectedTrain);
			SwapCommand = new MvxCommand(Swap);
			TappedRouteCommand = new MvxCommand<Route>(SetRoute);
		}

		#region actions

		public void Init()
		{
			InitializePropertiesToDefaultState();

			Trains = _appSettings.LastRequestedTrains;
			LastRoutes = new List<Route>(_appSettings?.LastRoutes);
			AboutItems = _appSettings.About;
			SelectedVariant = VariantOfSearch[0];
			SearchDate = DateTimeOffset.Now;
			LastRoutes = new List<Route>();
		}

		private async Task SearchTrainAsync(string from, string to)
		{
			var exception = CheckInput(SearchDate, from, to);

			if (!string.IsNullOrEmpty(exception))
			{
				await _userInteraction.AlertAsync(_localizationService.GetString(exception));
				return;
			}

			IsLoading = true;

			AddToLastRoutes(new Route { From = from, To = to });

			var schedule = (await _search.GetTrainSchedule(
				_appSettings.AutoCompletion.First(x => x.Value == from),
				_appSettings.AutoCompletion.First(x => x.Value == to),
				SearchDate,
				SelectedVariant)).ToList();

			if (schedule.Any())
			{
				_appSettings.LastRequestedTrains = schedule.ToList();
				_appSettings.UpdatedLastRequest = new LastRequest
				{
					Route = new Route { From = from, To = to },
					SelectionMode = SelectedVariant,
					Date = SearchDate
				};
				_sorage.Save(_appSettings.UpdatedLastRequest, Defines.Restoring.UpdateLastRequest);

				ShowViewModel<ScheduleViewModel>(new { param = _jsonConverter.Serialize(schedule) });

				_analytics.SentEvent(Defines.Analytics.VariantOfSearch, SelectedVariant);
			}
			else
			{
				await _userInteraction.AlertAsync(_localizationService.GetString("TrainsNotFound"));
			}

			IsLoading = false;
		}

		private async void UpdateLastRequest()
		{
			if (_appSettings.UpdatedLastRequest == null)
			{
				return;
			}

			IsLoading = true;

			var trains =
				await
					_search.GetTrainSchedule(
						_appSettings.AutoCompletion.First(x => x.Value == _appSettings.UpdatedLastRequest.Route.From),
						_appSettings.AutoCompletion.First(x => x.Value == _appSettings.UpdatedLastRequest.Route.To),
						_appSettings.UpdatedLastRequest.Date,
						_appSettings.UpdatedLastRequest.SelectionMode);

			if (trains == null)
			{
				await _userInteraction.AlertAsync(_localizationService.GetString("InternetConnectionError"));
			}
			else
			{
				_appSettings.UpdatedLastRequest.Date = DateTimeOffset.Now;
				_sorage.Save(_appSettings.UpdatedLastRequest, Defines.Restoring.UpdateLastRequest);
				RaisePropertyChanged(() => LastUpdateTime);
				Trains = trains;
				_sorage.Save(Trains, Defines.Restoring.LastTrainList);
			}

			IsLoading = false;
		}

		private void ClickItem(TrainModel train)
		{
			if (train == null)
			{
				return;
			}

			ShowViewModel<InformationViewModel>(new { param = _jsonConverter.Serialize(train) });
		}

		private void GoToHelpPage()
		{
			ShowViewModel<HelpViewModel>();
		}

		private void ClickAboutItem(About selectedAboutItem)
		{
			if (selectedAboutItem != null)
				AboutItemsActions[selectedAboutItem.Item]();
		}

		private void Swap()
		{
			var tmp = From;
			From = To;
			To = tmp;
		}

		private void AddToLastRoutes(Route route)
		{
			LastRoutes.Insert(0, route);
			_sorage.Save(LastRoutes, Defines.Restoring.LastRoutes);
		}

		private void SetRoute(Route route)
		{
			if (route != null)
			{
				From = route.From;
				To = route.To;
			}
		}

		public void UpdateAutoSuggestions(string str)
		{
			if (string.IsNullOrWhiteSpace(str))
			{
				return;
			}

			var station = str.Trim();

			var result =
				_appSettings.AutoCompletion.Where(x => x.Value.IndexOf(station, StringComparison.OrdinalIgnoreCase) >= 0)
					.Select(x => x.Value)
					.ToList();

			if (result.Count == 1 && result[0] == station)
			{
				AutoSuggestions.Clear();
			}
			else
			{
				AutoSuggestions = result;
			}
		}

		public string CheckInput(DateTimeOffset searchDate, string from, string to)
		{
			if ((searchDate.Date - DateTime.Now).Days < 0)
			{
				return _localizationService.GetString("DateUpTooLater");
			}
			if (searchDate.Date > DateTime.Now.AddDays(45))
			{
				return _localizationService.GetString("DateTooBig");
			}

			if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) ||
				!(_appSettings.AutoCompletion.Any(x => x.Value == from.Trim()) &&
				  _appSettings.AutoCompletion.Any(x => x.Value == to.Trim())))
			{
				return _localizationService.GetString("IncorrectInput");
			}

			if (NetworkInterface.GetIsNetworkAvailable())
			{
				return _localizationService.GetString("ConectionError");
			}

			return null;
		}

		public async void NotifyAboutSelectedTrain(TrainModel train)
		{
			var reminder = await _notificationService.AddTrainToNotification(train, _appSettings.Reminder);
			await _userInteraction.AlertAsync(string.Format(_localizationService.GetString("NotifyTrainMessage"), reminder));
		}

		#region restoreResources

		private void InitializePropertiesToDefaultState()
		{
			VariantOfSearch = new List<string>
			{
				//_localizationService.GetString("Yesterday"),
				_localizationService.GetString("Today"),
				_localizationService.GetString("Tommorow"),
				_localizationService.GetString("OnAllDays"),
				_localizationService.GetString("OnDay")
			};

			AboutItemsActions = new Dictionary<AboutPicture, Action>
			{
				{AboutPicture.ABOUTAPP, () => ShowViewModel<AboutViewModel>()},
				{
					AboutPicture.MAIL,
					() =>
						_email.ComposeEmail("sampir.fiesta@gmail.com", string.Empty, _localizationService.GetString("Feedback"), string.Empty, false)
				},
				{AboutPicture.MARKET, () => _marketPlace.GoToMarket()},
				{AboutPicture.SETTINGS, () => ShowViewModel<SettingsViewModel>()},
				{AboutPicture.SHARE, () => ShowViewModel<ShareViewModel>()}
			};
		}

		#endregion

		#endregion
	}
}
