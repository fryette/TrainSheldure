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

		private readonly ISerializableService _serializable;

		private readonly ISearchService _search;

		private readonly INotificationService _notificationService;

		private readonly IUserInteraction _userInteraction;

		private readonly ILocalizationService _localizationService;

		private readonly IJsonConverter _jsonConverter;

		#endregion

		#region commands

		public MvxCommand<About> TappedAboutItemCommand { get; private set; }
		public IMvxCommand GoToHelpCommand { get; private set; }
		public MvxCommand<TrainModel> SelectTrainCommand { get; private set; }
		public IMvxCommand UpdateLastRequestCommand { get; private set; }
		public IMvxCommand SearchTrainCommand { get; private set; }
		public IMvxCommand SwapCommand { get; private set; }
		public MvxCommand<Route> TappedRouteCommand { get; private set; }
		public MvxCommand<TrainModel> NotifyAboutSelectedTrainCommand { get; private set; }

		#endregion

		#region ctor

		public MainViewModel(
			ISerializableService serializable,
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
			_serializable = serializable;
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
			SearchTrainCommand = new MvxCommand(() => SearchTrain(From?.Trim(), To?.Trim()));

			SwapCommand = new MvxCommand(Swap);
			TappedRouteCommand = new MvxCommand<Route>(SetRoute);

			NotifyAboutSelectedTrainCommand = new MvxCommand<TrainModel>(NotifyAboutSelectedTrain);
		}

		#endregion

		#region properties

		private Dictionary<AboutPicture, Action> AboutItemsActions { get; set; }
		public IEnumerable<About> AboutItems { get; set; }

		private List<Route> _lastRoutes;

		public List<Route> LastRoutes
		{
			get { return _lastRoutes; }
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

		private List<string> _variantOfSearch;

		public List<string> VariantOfSearch
		{
			get { return _variantOfSearch; }
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
				if (_appSettings.UpdatedLastRequest == null)
					return null;
				var date = DateTimeOffset.Now - _appSettings.UpdatedLastRequest.Date;
				return _localizationService.GetString("Updated") +
					   (date.TotalMinutes > 1
						   ? (date.Hours > 1
							   ? date.Hours + _localizationService.GetString("Hour")
							   : date.Minutes + _localizationService.GetString("Min")) + _localizationService.GetString("Ago")
						   : _localizationService.GetString("JustNow"));
			}
		}

		private string _selectedDate;

		public string SelectedVariant
		{
			get { return _selectedDate; }

			set
			{
				_selectedDate = value;
				RaisePropertyChanged(() => SelectedVariant);
			}
		}

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

		private static IEnumerable<TrainModel> _trains;

		public IEnumerable<TrainModel> Trains
		{
			get { return _trains; }
			set
			{
				_trains = value;
				RaisePropertyChanged(() => Trains);
			}
		}

		#endregion

		#region actions

		public void Init()
		{
			RestoreUiBinding();

			Trains = _appSettings.LastRequestTrain;
			LastRoutes = new List<Route>(_appSettings?.LastRoutes);
			AboutItems = _appSettings.About;
			SelectedVariant = VariantOfSearch[0];
		}

		private async void SearchTrain(string from, string to)
		{
			if (await CheckInput(Datum, from, to))
				return;
			IsTaskRun = true;
			AddToLastRoutes(new Route { From = from, To = to });
			var schedule = (await _search.GetTrainSchedule(
				_appSettings.AutoCompletion.First(x => x.Value == @from),
				_appSettings.AutoCompletion.First(x => x.Value == to),
				Datum,
				SelectedVariant)).ToList();

			if (schedule.Any())
			{
				_appSettings.LastRequestTrain = schedule.ToList();
				_appSettings.UpdatedLastRequest = new LastRequest
				{
					Route = new Route { From = from, To = to },
					SelectionMode = SelectedVariant,
					Date = Datum
				};
				_serializable.Serialize(_appSettings.UpdatedLastRequest, Defines.Restoring.UpdateLastRequest);
				_serializable.Serialize(schedule, Defines.Restoring.LastTrainList);
				ShowViewModel<ScheduleViewModel>(new { param = _jsonConverter.Serialize(schedule) });

				_analytics.SentEvent(Defines.Analytics.VariantOfSearch, SelectedVariant);
			}

			IsTaskRun = false;
		}

		private async void UpdateLastRequest()
		{
			if (_appSettings.UpdatedLastRequest == null)
			{
				return;
			}

			IsTaskRun = true;

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
				_serializable.Serialize(_appSettings.UpdatedLastRequest, Defines.Restoring.UpdateLastRequest);
				RaisePropertyChanged(() => LastUpdateTime);
				Trains = trains;
				_serializable.Serialize(Trains, Defines.Restoring.LastTrainList);
			}

			IsTaskRun = false;
		}

		private void ClickItem(TrainModel train)
		{
			if (train == null)
				return;
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
			var routes = new List<Route> { route };

			if (LastRoutes == null)
			{
				LastRoutes = new List<Route>();
			}

			routes.AddRange(LastRoutes);

			_appSettings.LastRoutes =
				LastRoutes = routes.Take(20).GroupBy(x => new { x.From, x.To }).Select(g => g.First()).ToList();

			_serializable.Serialize(LastRoutes, Defines.Restoring.LastRoutes);
		}

		private void SetRoute(Route route)
		{
			if (route == null)
				return;
			From = route.From;
			To = route.To;
		}

		public void UpdateAutoSuggestions(string str)
		{
			var station = str.Trim();
			if (IsNullOrEmpty(station))
				AutoSuggestions = null;
			AutoSuggestions =
				_appSettings.AutoCompletion.Where(x => x.Value.IndexOf(station, StringComparison.OrdinalIgnoreCase) >= 0)
					.Select(x => x.Value)
					.ToList();
			if (AutoSuggestions.Count == 1 && AutoSuggestions[0] == station)
				AutoSuggestions = null;
		}

		public async Task<bool> CheckInput(DateTimeOffset datum, string from, string to)
		{
			if ((datum.Date - DateTime.Now).Days < 0)
			{
				await _userInteraction.AlertAsync(_localizationService.GetString("DateUpTooLater"));
				return true;
			}
			if (datum.Date > DateTime.Now.AddDays(45))
			{
				await _userInteraction.AlertAsync(_localizationService.GetString("DateTooBig"));
				return true;
			}

			if (IsNullOrEmpty(from) || IsNullOrEmpty(to) ||
				!(_appSettings.AutoCompletion.Any(x => x.Value == from.Trim()) &&
				  _appSettings.AutoCompletion.Any(x => x.Value == to.Trim())))
			{
				await _userInteraction.AlertAsync(_localizationService.GetString("IncorrectInput"));
				return true;
			}

			if (NetworkInterface.GetIsNetworkAvailable())
				return false;
			await _userInteraction.AlertAsync(_localizationService.GetString("ConectionError"));
			return true;
		}

		public async void NotifyAboutSelectedTrain(TrainModel train)
		{
			var reminder = await _notificationService.AddTrainToNotification(train, _appSettings.Reminder);
			await _userInteraction.AlertAsync(Format(_localizationService.GetString("NotifyTrainMessage"), reminder));
		}

		#region restoreResources

		private void RestoreUiBinding()
		{
			VariantOfSearch = new List<string>
			{
				//_localizationService.GetString("Yesterday"),
				_localizationService.GetString("Today"),
				_localizationService.GetString("Tommorow"),
				//_localizationService.GetString("OnAllDays"),
				_localizationService.GetString("OnDay")
			};

			AboutItemsActions = new Dictionary<AboutPicture, Action>
			{
				{AboutPicture.ABOUTAPP, () => ShowViewModel<AboutViewModel>()},
				{
					AboutPicture.MAIL,
					() =>
						_email.ComposeEmail("sampir.fiesta@gmail.com", Empty, _localizationService.GetString("Feedback"), Empty, false)
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
