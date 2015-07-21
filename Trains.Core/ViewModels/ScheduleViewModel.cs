using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Entities;
using Trains.Core.Services.Interfaces;

namespace Trains.Core.ViewModels
{
	public class ScheduleViewModel : MvxViewModel
	{
		#region readonlyProperty

		private readonly IAppSettings _appSettings;
		private readonly IFavoriteManageService _manageFavoriteRequest;
		private readonly IAnalytics _analytics;
		private readonly ISearchService _search;

		#endregion

		#region command

		public IMvxCommand GoToHelpPageCommand { get; private set; }
		public MvxCommand<Train> SelectTrainCommand { get; private set; }
		public IMvxCommand SearchReverseRouteCommand { get; private set; }
		public IMvxCommand AddToFavoriteCommand { get; private set; }
		public IMvxCommand DeleteInFavoriteCommand { get; private set; }

		#endregion

		#region ctor

		public ScheduleViewModel(IAppSettings appSettings, IFavoriteManageService manageFavoriteRequest, IAnalytics analytics, ISearchService search)
		{
			_manageFavoriteRequest = manageFavoriteRequest;
			_appSettings = appSettings;
			_analytics = analytics;
			_search = search;

			SearchReverseRouteCommand = new MvxCommand(SearchReverseRoute);
			AddToFavoriteCommand = new MvxCommand(AddToFavorite);
			DeleteInFavoriteCommand = new MvxCommand(DeleteInFavorite);
			GoToHelpPageCommand = new MvxCommand(GoToHelpPage);
			SelectTrainCommand = new MvxCommand<Train>(ClickItem);
		}

		#endregion

		#region properties

		#region UIproperties

		public string ReverseAppBar { get; set; }
		public string SaveAppBar { get; set; }
		public string DeleteAppBar { get; set; }
		public string HelpAppBar { get; set; }
		public string Update { get; set; }

		#endregion


		private string From { get; set; }
		private string To { get; set; }
		/// <summary>
		/// Used to display favorite icon.
		/// </summary> 
		private bool _isVisibleFavoriteIcon;
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
		/// Used to display unfavorite icon.
		/// </summary> 
		private bool _isVisibleUnFavoriteIcon;
		public bool IsVisibleUnFavoriteIcon
		{
			get
			{
				return _isVisibleUnFavoriteIcon;
			}

			set
			{
				_isVisibleUnFavoriteIcon = value;
				RaisePropertyChanged(() => IsVisibleUnFavoriteIcon);
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
			SetManageFavoriteButton();
		}

		private async void SearchReverseRoute()
		{
			IsSearchStart = true;
			Trains = await _search.GetTrainSchedule(_appSettings.AutoCompletion.First(x => x.Value == To),
							_appSettings.AutoCompletion.First(x => x.Value == From),
							_appSettings.UpdatedLastRequest.Date, _appSettings.UpdatedLastRequest.SelectionMode);
			SwapStopPoint();
			Request = From + " - " + To;
			SetManageFavoriteButton();

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

		private void SetManageFavoriteButton()
		{
			if (_appSettings.FavoriteRequests == null) SetVisibilityToFavoriteIcons(true, false);
			else if (_appSettings.FavoriteRequests.Any(x => x.Route.From == From && x.Route.To == To))
				SetVisibilityToFavoriteIcons(false, true);
			else SetVisibilityToFavoriteIcons(true, false);
		}

		/// <summary>
		/// Saves the entered route to favorite.
		/// </summary>
		private void AddToFavorite()
		{
			if (_manageFavoriteRequest.AddToFavorite(From, To))
			{
				SetVisibilityToFavoriteIcons(false, true);
				_analytics.SentEvent(Constants.Analytics.AddToFavorite);

			}
		}

		/// <summary>
		/// Deletes the entered route visas favorite.
		/// </summary>
		private void DeleteInFavorite()
		{
			if (_manageFavoriteRequest.DeleteRoute(From, To))
				SetVisibilityToFavoriteIcons(true, false);
		}

		/// <summary>
		/// Change visibility of favorite and unfavorite buttons when user add route to favorite or delete this route.
		/// </summary>
		private void SetVisibilityToFavoriteIcons(bool favorite, bool unfavorite)
		{
			IsVisibleFavoriteIcon = favorite;
			IsVisibleUnFavoriteIcon = unfavorite;
		}

		private void RestoreUiBinding()
		{
			ReverseAppBar = ResourceLoader.Instance.Resource["ReverseAppBar"];
			Update = ResourceLoader.Instance.Resource["Update"];
			SaveAppBar = ResourceLoader.Instance.Resource["SaveAppBar"];
			DeleteAppBar = ResourceLoader.Instance.Resource["DeleteAppBar"];
			HelpAppBar = ResourceLoader.Instance.Resource["HelpAppBar"];
		}

		#endregion
	}
}