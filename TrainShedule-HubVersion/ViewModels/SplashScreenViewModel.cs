using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System.Threading;
using Windows.UI.Core;
using Caliburn.Micro;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.App.ViewModels
{
    public class SplashScreenViewModel : Screen
    {
        #region constants
        private const string FavoriteString = "favoriteRequests";
        private const int AddedProgress = 20;
        private const string UpdateLastRequestString = "updateLastRequst";
        #endregion
        #region properties

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
        public SplashScreenViewModel(INavigationService navigationService, ISerializableService serializable, ISearchService search)
        {
            _navigationService = navigationService;
            _serializable = serializable;
            _search = search;
        }

        #endregion

        #region action

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        protected override void OnActivate()
        {
            StartedActions();
        }

        /// <summary>
        /// Download static resources
        /// </summary>
        private void StartedActions()
        {
            var asyncAction = ThreadPool.RunAsync(async workItem =>
            {
                SavedItems.LastRequests = await Task.Run(() => _serializable.GetLastRequests("lastRequests"));
                Progress += AddedProgress;
                SavedItems.FavoriteRequests = await Task.Run(() => _serializable.GetLastRequests("favoriteRequests"));
                Progress += AddedProgress;
                SavedItems.AutoCompletion = await Task.Run(() => _search.GetCountryStopPoint());
                Progress += AddedProgress;
                SavedItems.UpdatedLastRequest = await Task.Run(() => _serializable.ReadObjectFromXmlFileAsync<LastRequest>(UpdateLastRequestString));
                Progress += AddedProgress;

            });

            asyncAction.Completed = async (asyncInfo, asyncStatus) => await (CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                _navigationService.NavigateToViewModel<MainViewModel>()));
        }

        #endregion
    }
}
