using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Entities;
using Trains.Core.Services.Interfaces;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
	public class ScheduleViewModel : MvxViewModel
	{
		#region readonlyProperty

		private readonly IAppSettings _appSettings;
		private readonly IAnalytics _analytics;
		private readonly ISearchService _search;
		private readonly ISerializableService _serializable;
		private readonly INotificationService _notificationService;

		#endregion

		#region command

		public IMvxCommand GoToHelpPageCommand { get; private set; }
		public MvxCommand<Train> SelectTrainCommand { get; private set; }
		public IMvxCommand SearchReverseRouteCommand { get; private set; }
		public IMvxCommand AddToFavoriteCommand { get; private set; }

		#endregion

		#region ctor

		public ScheduleViewModel(IAppSettings appSettings, ISerializableService serializableService, IAnalytics analytics, ISearchService search, INotificationService notificationService)
		{
			_serializable = serializableService;
			_appSettings = appSettings;
			_analytics = analytics;
			_search = search;
			_notificationService = notificationService;

			SearchReverseRouteCommand = new MvxCommand(SearchReverseRoute);
			AddToFavoriteCommand = new MvxCommand(AddToFavorite);
			GoToHelpPageCommand = new MvxCommand(GoToHelpPage);
			SelectTrainCommand = new MvxCommand<Train>(ClickItem);
		}

		#endregion

		#region properties

		#region UIproperties

		public string ReverseAppBar { get; set; }
		public string SaveAppBar { get; set; }
		public string HelpAppBar { get; set; }
		public string Update { get; set; }

		#endregion


		private string From { get; set; }
		private string To { get; set; }
		/// <summary>
		/// Used to display favorite icon.
		/// </summary> 
		private bool _isVisibleFavoriteIcon = true;
		public bool IsVisibleFavoriteIcon
		{
			get
			{
				return _isVisibleFavoriteIcon;
			}

			set
			{
				_isVisibleFavoriteIcon = value;
				RaisePropertyChanged(() => IsVisibleFavoriteIcon);
			}
		}

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

		/// <summary>
		/// Ñontains information on all trains on the route selected by the user.
		/// </summary> 
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

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// Set the default parameter of some properties.
		/// </summary>
		public void Init(string param)
		{
			RestoreUiBinding();
			Trains = JsonConvert.DeserializeObject<List<Train>>(param);
			From = _appSettings.UpdatedLastRequest.Route.From;
			To = _appSettings.UpdatedLastRequest.Route.To;
			Request = From + " - " + To;
			ManageFavoriteButton();
		}

		private async void SearchReverseRoute()
		{
			IsSearchStart = true;
			Trains = await _search.GetTrainSchedule(_appSettings.AutoCompletion.First(x => x.Value == To),
							_appSettings.AutoCompletion.First(x => x.Value == From),
							_appSettings.UpdatedLastRequest.Date, _appSettings.UpdatedLastRequest.SelectionMode);
			SwapStopPoint();
			Request = From + " - " + To;
			ManageFavoriteButton();

			IsSearchStart = false;
		}

		private void SwapStopPoint()
		{
			var temp = From;
			From = To;
			To = temp;
		}

		/// <summary>
		/// Invoked when the user selects his train of interest.
		/// Go to the information page which show additional information about this train.
		/// </summary>
		/// <param name="train">Data that describes user-selected train(prices,seats,stop points and other)</param>
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

		private void ManageFavoriteButton()
		{
			if (_appSettings.FavoriteRequests != null)
				IsVisibleFavoriteIcon = !_appSettings.FavoriteRequests.Any(x => x.Route.From == From && x.Route.To == To);
		}

		/// <summary>
		/// Saves the entered route to favorite.
		/// </summary>
		private void AddToFavorite()
		{
			if (_appSettings.FavoriteRequests == null) _appSettings.FavoriteRequests = new List<LastRequest>();
			if (_appSettings.FavoriteRequests.Any(x => x.Route.From == From && x.Route.To == To))
				return;

			_appSettings.FavoriteRequests.Add(new LastRequest { Route = new Route { From = From, To = To } });
			_serializable.Serialize(_appSettings.FavoriteRequests, Defines.Restoring.FavoriteRequests);

			IsVisibleFavoriteIcon = false;

			_analytics.SentEvent(Defines.Analytics.AddToFavorite);
		}

		public async void NotifyAboutSelectedTrain(Train train)
		{
			await _notificationService.AddTrainToNotification(train);
		}

		private void RestoreUiBinding()
		{
			ReverseAppBar = ResourceLoader.Instance.Resource["ReverseAppBar"];
			Update = ResourceLoader.Instance.Resource["Update"];
			SaveAppBar = ResourceLoader.Instance.Resource["SaveAppBar"];
			HelpAppBar = ResourceLoader.Instance.Resource["HelpAppBar"];
		}

		#endregion
	}
}