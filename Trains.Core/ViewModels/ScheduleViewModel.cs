using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.MvvmCross.ViewModels;
using Trains.Entities;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Infrastructure.Interfaces.Services;
using static System.String;

namespace Trains.Core.ViewModels
{
	public class ScheduleViewModel : MvxViewModel
	{
		#region readonlyProperty

		private readonly IAppSettings _appSettings;
		private readonly ISearchService _search;
		private readonly INotificationService _notificationService;
		private readonly IUserInteraction _userInteraction;
		private readonly ILocalizationService _localizationService;
		private readonly IJsonConverter _jsonConverter;

		#endregion

		#region command

		public ICommand GoToHelpPageCommand { get; private set; }
		public MvxCommand<Train> SelectTrainCommand { get; private set; }
		public ICommand SearchReverseRouteCommand { get; private set; }
		public MvxCommand<Train> NotifyAboutSelectedTrainCommand { get; set; }
		public MvxCommand<Train> BookingSelectedTrainCommand { get; private set; }

		#endregion

		#region ctor

		public ScheduleViewModel(IAppSettings appSettings,
			ISearchService search,
			INotificationService notificationService,
			IUserInteraction userInteraction,
			ILocalizationService localizationService,
			IJsonConverter jsonConverter)
		{
			_appSettings = appSettings;
			_search = search;
			_notificationService = notificationService;
			_userInteraction = userInteraction;
			_localizationService = localizationService;
			_jsonConverter = jsonConverter;

			SearchReverseRouteCommand = new MvxCommand(SearchReverseRoute);
			GoToHelpPageCommand = new MvxCommand(() => ShowViewModel<HelpViewModel>());
			SelectTrainCommand = new MvxCommand<Train>(ClickItem);
			NotifyAboutSelectedTrainCommand = new MvxCommand<Train>(NotifyAboutSelectedTrain);

			BookingSelectedTrainCommand = new MvxCommand<Train>(train => ShowViewModel<BookingViewModel>(new
			{
				param = _jsonConverter.Serialize(train)
			}));
		}

		#endregion

		#region properties

		private string From { get; set; }
		private string To { get; set; }

		private bool _isSearchStart;
		public bool IsSearchStart
		{
			get
			{
				return _isSearchStart;
			}

			set
			{
				_isSearchStart = value;
				RaisePropertyChanged(() => IsSearchStart);
			}
		}
		private List<Train> _trains;
		public List<Train> Trains
		{
			get
			{
				return _trains;
			}
			set
			{
				_trains = value;
				RaisePropertyChanged(() => Trains);
			}
		}

		private string _request;
		public string Request
		{
			get
			{
				return _request;
			}
			set
			{
				_request = value;
				RaisePropertyChanged(() => Request);
			}
		}

		#endregion

		#region action

		public void Init(string param)
		{
			Trains = _jsonConverter.Deserialize<List<Train>>(param);
			From = _appSettings.UpdatedLastRequest.Route.From;
			To = _appSettings.UpdatedLastRequest.Route.To;
			Request = From + " - " + To;
		}

		private async void SearchReverseRoute()
		{
			IsSearchStart = true;
			Trains = await _search.GetTrainSchedule(_appSettings.AutoCompletion.First(x => x.Value == To),
							_appSettings.AutoCompletion.First(x => x.Value == From),
							_appSettings.UpdatedLastRequest.Date, _appSettings.UpdatedLastRequest.SelectionMode);
			SwapStopPoint();
			Request = From + " - " + To;

			IsSearchStart = false;
		}

		private void SwapStopPoint()
		{
			var temp = From;
			From = To;
			To = temp;
		}

		private void ClickItem(Train train)
		{
			if (train == null)
			{
				return;
			}

			ShowViewModel<InformationViewModel>(new { param = _jsonConverter.Serialize(train) });
		}

		public async void NotifyAboutSelectedTrain(Train train)
		{
			var reminder = await _notificationService.AddTrainToNotification(train, _appSettings.Reminder);
			await _userInteraction.AlertAsync(Format(_localizationService.GetString("NotifyTrainMessage"), reminder));
		}

		#endregion
	}
}