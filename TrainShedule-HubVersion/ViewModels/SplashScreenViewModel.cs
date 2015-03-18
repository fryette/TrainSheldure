using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System.Threading;
using Windows.UI.Core;
using Caliburn.Micro;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;

namespace Trains.App.ViewModels
{
    public class SplashScreenViewModel : Screen
    {
        #region constants
        private const string FavoriteString = "favoriteRequests";
        private const string LastRequestString = "lastRequests";
        private const int AddedProgress = 20;
        private const string UpdateLastRequestString = "updateLastRequst";
        private const string IsFirstStartString = "isFirstStart";
        private const string FirstMessageStartString = "Привет, обращаюсь к вам, от лица разработчика данного приложения. Многие ждали обновления, " +
                                                       "внедрения новых функций, удобства, покупки билетов, без рекламы. Я обращаюсь" +
                                                       " ко всем,кто только начал пользоваться, и продолжает пользоваться данным приложением, если вы хотите новых функций и " +
                                                       "исправления багов/глюков,оставьте отзыв в маркете, в противном случае, я прекращу поддержку.Каждый может помочь. Спасибо за внимание.";

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
        protected override async void OnActivate()
        {
            CheckIsFirstStart();
            await Task.Run(async () =>
            {
                while (Progress < 80)
                {
                    Progress += 1;
                    await Task.Delay(5);
                }
            });
            await Task.Run(() => StartedActions());
        }

        /// <summary>
        /// Download static resources
        /// </summary>
        private void StartedActions()
        {
            var asyncAction = ThreadPool.RunAsync(async workItem =>
            {
                SavedItems.AutoCompletion = await Task.Run(() => _search.GetCountryStopPoint());
                Progress += AddedProgress;
                SavedItems.UpdatedLastRequest = await Task.Run(() => _serializable.ReadObjectFromXmlFileAsync<LastRequest>(UpdateLastRequestString));
                Progress += AddedProgress;

            });

            asyncAction.Completed = async (asyncInfo, asyncStatus) => await (CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                _navigationService.NavigateToViewModel<MainViewModel>()));
        }

        private async void CheckIsFirstStart()
        {
            if ((await _serializable.CheckIsFile(IsFirstStartString)))
            {
                await Task.Run(() => SerializationData());
            }
            else
            {
                ToolHelper.ShowMessageBox(FirstMessageStartString);
                _serializable.SerializeObjectToXml(true, IsFirstStartString);
                if (await _serializable.CheckIsFile(FavoriteString))
                    _serializable.DeleteFile(FavoriteString);
                if (await _serializable.CheckIsFile(LastRequestString))
                    _serializable.DeleteFile(LastRequestString);

            }
        }

        private async void SerializationData()
        {
            SavedItems.LastRequests = await Task.Run(() => _serializable.GetLastRequests(LastRequestString));
            Progress += AddedProgress;
            SavedItems.FavoriteRequests = await Task.Run(() => _serializable.GetLastRequests(FavoriteString));
            Progress += AddedProgress;
        }

        #endregion
    }
}
