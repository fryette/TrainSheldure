using System.Collections.Generic;
using Caliburn.Micro;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using TrainSearch.Entities;

namespace Trains.App.ViewModels
{
    /// <summary>
    /// Model by start page
    /// </summary>
    public class MainPageViewModel : Screen
    {
        #region readonlyProperties
        /// <summary>
        /// Used to get trains from the last request.
        /// </summary>
        private readonly ILastRequestTrainService _lastRequestTrain;

        /// <summary>
        /// Used to navigate between pages.
        /// </summary>
        private readonly INavigationService _navigationService;

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly ISerializableService _serializable;
        #endregion

        #region constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="lastRequestTrainService">Used to search deserialize trains from the last request.</param>
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        public MainPageViewModel(INavigationService navigationService, ILastRequestTrainService lastRequestTrainService, ISerializableService serializable)
        {
            _navigationService = navigationService;
            _lastRequestTrain = lastRequestTrainService;
            _serializable = serializable;
        }
        #endregion

        #region properties
        /// <summary>
        /// Keeps trains from the last request.
        /// </summary>
        private static IEnumerable<Train> _trains;
        public IEnumerable<Train> Trains
        {
            get { return _trains; }
            set
            {
                _trains = value;
                NotifyOfPropertyChange(() => Trains);
            }
        }
        #endregion

        #region action

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        protected override async void OnActivate()
        {
            SavedItems.LastRequests = await _serializable.GetLastRequests("lastRequests");
            SavedItems.FavoriteRequests = await _serializable.GetLastRequests("favoriteRequests");
            Trains = await _lastRequestTrain.GetTrains();
        }

        /// <summary>
        /// Go to page,where user must enter the stopping points.
        /// </summary>
        private void GoToSearch()
        {
            _navigationService.NavigateToViewModel<ItemPageViewModel>();
        }

        /// <summary>
        /// Invoked when the user selects his train of interest.
        /// </summary>
        /// <param name="train">Data that describes user-selected train(prices,seats,stop points,and other)</param>
        private void ClickItem(Train train)
        {
            _navigationService.NavigateToViewModel<InformationPageViewModel>(train);
        }
        /// <summary>
        /// Go to favorite routes page.
        /// </summary>
        private void GoToFavoriteList()
        {
            _navigationService.NavigateToViewModel<FavoritePageViewModel>();
        }

        /// <summary>
        /// Go to saved by user routes page.
        /// </summary>
        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpPageViewModel>();
        }
        #endregion
    }
}

