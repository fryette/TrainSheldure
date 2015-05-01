using System;
using System.Collections.Generic;
using System.Linq;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Email;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Core.Services.Interfaces;
using Trains.Entities;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class MainViewModel : BaseSearchViewModel
    {
        #region readonlyProperties

        private readonly IHttpService _httpService;

        private IAppSettings _appSettings;

        private readonly IMarketPlaceService _marketPlace;

        private readonly IAnalytics _analytics;

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly ISerializableService _serializable;


        /// <summary>
        /// Used to search train schedule.
        /// </summary>
        private readonly ISearchService _search;


        #endregion

        #region commands

        public MvxCommand<About> TappedAboutItemCommand { get; private set; }
        public IMvxCommand GoToFavoriteListCommand { get; private set; }
        public IMvxCommand GoToEditFavoriteCommand { get; private set; }
        public IMvxCommand GoToHelpCommand { get; private set; }
        public MvxCommand<Train> SelectTrainCommand { get; private set; }
        public MvxCommand<LastRequest> TappedFavoriteCommand { get; private set; }
        public IMvxCommand UpdateLastRequestCommand { get; private set; }
        public IMvxCommand SearchCommand { get; private set; }
        public IMvxCommand SwapCommand { get; private set; }

        #endregion

        #region ctor

        public MainViewModel(ISerializableService serializable, ISearchService search, IAppSettings appSettings, IMarketPlaceService marketPlace, IAnalytics analytics, IHttpService httpService)
        {
            _httpService = httpService;
            _analytics = analytics;
            _serializable = serializable;
            _search = search;
            _appSettings = appSettings;
            _marketPlace = marketPlace;

            TappedAboutItemCommand = new MvxCommand<About>(ClickAboutItem);
            GoToEditFavoriteCommand = new MvxCommand(GoToEditFavorite);
            GoToFavoriteListCommand = new MvxCommand(GoToFavoriteList);
            GoToHelpCommand = new MvxCommand(GoToHelpPage);
            SelectTrainCommand = new MvxCommand<Train>(ClickItem);
            UpdateLastRequestCommand = new MvxCommand(UpdateLastRequest);
            SearchCommand = new MvxCommand(() => Search(From, To));
            SwapCommand = new MvxCommand(Swap);
            TappedFavoriteCommand = new MvxCommand<LastRequest>(route =>
            {
                if (route == null) return;
                Search(route.From, route.To);
            });
        }

        #endregion

        #region properties

        public IEnumerable<About> AboutItems { get; set; }

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

        /// <summary>
        /// Stores variant of search.
        /// </summary> 
        private List<string> _variantOfSearch;
        public List<string> VariantOfSearch
        {
            get
            {
                return _variantOfSearch;
            }
            set
            {
                _variantOfSearch = value;
                RaisePropertyChanged(() => VariantOfSearch);
            }
        }

        /// <summary>
        /// Used to set code behind variant of search.
        /// </summary> 
        private string _selectedDate;
        public string SelectedVariant
        {
            get
            {
                return _selectedDate;
            }

            set
            {
                _selectedDate = value;
                IsOnDaySelected = SelectedVariant == ResourceLoader.Instance.Resource["OnDay"] ? true : false;
                RaisePropertyChanged(() => SelectedVariant);
            }
        }

        /// <summary>
        /// Contains stopping points satisfying user input.
        /// </summary> 
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

        /// <summary>
        /// Used to read and set start stop point.
        /// </summary> 
        private string _from;
        public string From
        {
            get { return _from; }
            set
            {
                _from = value;
                RaisePropertyChanged(() => From);
                AutoSuggestions = UpdateAutoSuggestions(From, _appSettings.AutoCompletion);
            }
        }

        /// <summary>
        /// Used to read and set start end point.
        /// </summary> 
        private string _to;
        public string To
        {
            get { return _to; }
            set
            {
                _to = value;
                RaisePropertyChanged(() => To);
                AutoSuggestions = UpdateAutoSuggestions(To, _appSettings.AutoCompletion);
            }
        }

        /// <summary>
        /// Used for process control.
        /// </summary>
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

        /// <summary>
        /// Used for process control.
        /// </summary>
        private bool _isBarDownloaded;

        public bool IsBarDownloaded
        {
            get { return _isBarDownloaded; }
            set
            {
                _isBarDownloaded = value;
                RaisePropertyChanged(() => IsBarDownloaded);
            }
        }

        /// <summary>
        /// Used for process download data control.
        /// </summary>
        private bool _isDownloadRun;
        public bool IsDownloadRun
        {
            get { return _isDownloadRun; }
            set
            {
                _isDownloadRun = value;
                RaisePropertyChanged(() => IsDownloadRun);
            }
        }

        /// <summary>
        /// Keeps trains from the last request.
        /// </summary>
        private static List<Train> _trains;

        public List<Train> Trains
        {
            get { return _trains; }
            set
            {
                _trains = value;
                RaisePropertyChanged(() => Trains);
            }
        }

        /// <summary>
        /// Object are stored custom routes.
        /// </summary>
        private IEnumerable<LastRequest> _favoriteRequests;
        public IEnumerable<LastRequest> FavoriteRequests
        {
            get { return _favoriteRequests; }
            set
            {
                _favoriteRequests = value;
                RaisePropertyChanged(() => FavoriteRequests);
            }
        }

        /// <summary>
        /// Last route
        /// </summary>
        private string _lastRoute;
        public string LastRoute
        {
            get { return _lastRoute; }
            set
            {
                _lastRoute = value;
                RaisePropertyChanged(() => LastRoute);
            }
        }

        private bool _isOnDaySelected;
        public bool IsOnDaySelected
        {
            get { return _isOnDaySelected; }
            set
            {
                _isOnDaySelected = value;
                RaisePropertyChanged(() => IsOnDaySelected);
            }
        }

        #endregion

        #region actions

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        public void Init()
        {
            IsDownloadRun = true;
            RestoreData();
            IsBarDownloaded = true;
            if (_appSettings.UpdatedLastRequest != null)
                LastRoute = String.Format("{0} - {1}", _appSettings.UpdatedLastRequest.From, _appSettings.UpdatedLastRequest.To);
            Trains = _appSettings.LastRequestTrain;
            FavoriteRequests = _appSettings.FavoriteRequests;
            AboutItems = _appSettings.About;
            SelectedVariant = VariantOfSearch[1];
            IsDownloadRun = false;
        }

        /// <summary>
        /// Searches for train schedules at a specified date in the specified mode.
        /// </summary>
        private async void Search(string from, string to)
        {
            if (IsTaskRun || await CheckInput(Datum, from, to, _appSettings.AutoCompletion)) return;
            IsTaskRun = true;
            var exception = false;
            List<Train> schedule = null;
            try
            {
                schedule = await _search.GetTrainSchedule(_appSettings.AutoCompletion.First(x => x.UniqueId == from), _appSettings.AutoCompletion.First(x => x.UniqueId == to), Datum, SelectedVariant);
            }
            catch (Exception e)
            {
                _analytics.SentException(e.Message);
                exception = true;
            }
            if (exception)
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["TrainsNotFound"]);
            }
            else
            {
                _appSettings.LastRequestTrain = schedule;
                _appSettings.UpdatedLastRequest = new LastRequest { From = from, To = to, SelectionMode = SelectedVariant, Date = Datum };
                 _serializable.Serialize(_appSettings.UpdatedLastRequest, Constants.UpdateLastRequest);
                 _serializable.Serialize(schedule, Constants.LastTrainList);
                ShowViewModel<ScheduleViewModel>(new { param = JsonConvert.SerializeObject(schedule) });
            }
            _analytics.SentEvent(Constants.VariantOfSearch, SelectedVariant);
            IsTaskRun = false;
        }

        /// <summary>
        /// Invoked when the user selects his train of interest.
        /// </summary>
        /// <param name="train">Data that describes user-selected train(prices,seats,stop points,and other)</param>
        private void ClickItem(Train train)
        {
            if (train == null) return;
            ShowViewModel<InformationViewModel>(new { param = JsonConvert.SerializeObject(train) });
        }

        /// <summary>
        /// Go to favorite routes page.
        /// </summary>
        private void GoToFavoriteList()
        {
            ShowViewModel<EditFavoriteRoutesViewModel>();
        }

        /// <summary>
        /// Go to saved by user routes page.
        /// </summary>
        private void GoToHelpPage()
        {
            ShowViewModel<HelpViewModel>();
        }

        /// <summary>
        /// Used to manage user-saved routes.
        /// </summary>
        private async void GoToEditFavorite()
        {
            if (_appSettings.FavoriteRequests == null || !_appSettings.FavoriteRequests.Any())
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["EditFavoriteMessageError"]);
            else
                ShowViewModel<EditFavoriteRoutesViewModel>();
        }

        /// <summary>
        /// Update last route
        /// </summary>
        private async void UpdateLastRequest()
        {
            if (IsTaskRun) return;
            if (_appSettings.UpdatedLastRequest == null) return;
            IsTaskRun = true;

            var trains = await _search.GetTrainSchedule(_appSettings.AutoCompletion.First(x => x.UniqueId == _appSettings.UpdatedLastRequest.From),
                _appSettings.AutoCompletion.First(x => x.UniqueId == _appSettings.UpdatedLastRequest.To),
                _appSettings.UpdatedLastRequest.Date, _appSettings.UpdatedLastRequest.SelectionMode);

            if (trains == null)
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["InternetConnectionError"]);
            else
            {
                Trains = trains;
                 _serializable.Serialize(Trains, Constants.LastTrainList);
            }

            IsTaskRun = false;
        }

        private void ClickAboutItem(About selectedAboutItem)
        {
            if (selectedAboutItem == null) return;
            if (selectedAboutItem.Item == AboutPicture.AboutApp)
                ShowViewModel<AboutViewModel>();
            else if (selectedAboutItem.Item == AboutPicture.Mail)
                Mvx.Resolve<IMvxComposeEmailTask>().ComposeEmail("sampir.fiesta@gmail.com", string.Empty, ResourceLoader.Instance.Resource["Feedback"], String.Empty, false);
            else if (selectedAboutItem.Item == AboutPicture.Market)
                _marketPlace.GoToMarket();
            else
                ShowViewModel<SettingsViewModel>();
        }

        /// <summary>
        /// Swaped From and To properties.
        /// </summary>
        private void Swap()
        {
            var tmp = From;
            From = To;
            To = tmp;
        }

        private void RestoreData()
        {
            if (_appSettings.AutoCompletion == null)
            {
                var appSettings= _serializable.Desserialize<AppSettings>(Constants.AppSettings);

                _appSettings.AutoCompletion = appSettings.AutoCompletion;
                _appSettings.About = appSettings.About;
                _appSettings.HelpInformation = appSettings.HelpInformation;
                _appSettings.CarriageModel = appSettings.CarriageModel;

                _appSettings.FavoriteRequests =  _serializable.Desserialize<List<LastRequest>>(Constants.FavoriteRequests);
                _appSettings.UpdatedLastRequest =  _serializable.Desserialize<LastRequest>(Constants.UpdateLastRequest);
                _appSettings.LastRequestTrain =  _serializable.Desserialize<List<Train>>(Constants.LastTrainList);
            }

            VariantOfSearch = new List<string>
                {
                    ResourceLoader.Instance.Resource["Yesterday"],
                    ResourceLoader.Instance.Resource["Today"],
                    ResourceLoader.Instance.Resource["Tommorow"],
                    ResourceLoader.Instance.Resource["OnAllDays"],
                    ResourceLoader.Instance.Resource["OnDay"]
                };
        }

        #endregion
    }
}
