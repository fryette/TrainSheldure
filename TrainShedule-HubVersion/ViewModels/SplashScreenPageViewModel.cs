using Caliburn.Micro;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.App.ViewModels
{
    public class SplashScreenPageViewModel : Screen
    {
        /// <summary>
        /// Used to navigate between pages.
        /// </summary>
        private readonly INavigationService _navigationService;

        /// <summary>
        /// Used to display stop points and time of arrival and time of departure.
        /// </summary> 
        private readonly ISerializableService _serializable;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        public SplashScreenPageViewModel(INavigationService navigationService, ISerializableService serializable)
        {
            _navigationService = navigationService;
            _serializable = serializable;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        protected override async void OnActivate()
        {
            SavedItems.LastRequests = await _serializable.GetLastRequests("lastRequests");
            SavedItems.FavoriteRequests = await _serializable.GetLastRequests("favoriteRequests");
            _navigationService.NavigateToViewModel<MainPageViewModel>();
        }
    }
}
