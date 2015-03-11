using Windows.ApplicationModel.Core;
using Windows.System.Threading;
using Windows.UI.Core;
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
        /// For progress reporting.
        /// </summary>
        private uint _progress;
        public uint Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                NotifyOfPropertyChange(() => Progress);
            }
        }

        /// <summary>
        /// Used to display stop points and time of arrival and time of departure.
        /// </summary> 
        private readonly ISerializableService _serializable;

        /// <summary>
        /// Used to search train schedule.
        /// </summary>
        private readonly ISearchService _search;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        /// <param name="search">Used to search train schedule.</param>
        public SplashScreenPageViewModel(INavigationService navigationService, ISerializableService serializable, ISearchService search)
        {
            _navigationService = navigationService;
            _serializable = serializable;
            _search = search;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        protected override void OnActivate()
        {
            StartedActions();
        }

        private void StartedActions()
        {
            var asyncAction = ThreadPool.RunAsync(async workItem =>
            {
                SavedItems.LastRequests = await _serializable.GetLastRequests("lastRequests");
                Progress += 33;
                SavedItems.FavoriteRequests = await _serializable.GetLastRequests("favoriteRequests");
                Progress += 33;
                SavedItems.AutoCompletion = await _search.GetCountryStopPoint();
                Progress += 33;
            });

            asyncAction.Completed = (asyncInfo, asyncStatus) => CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => 
                _navigationService.NavigateToViewModel<MainPageViewModel>());
        }
    }
}
