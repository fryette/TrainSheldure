using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Email;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Entities;
using Trains.Model.Entities;
using Trains.Resources;
using Trains.Services.Interfaces;

namespace Trains.Core.ViewModels
{
    public class MainViewModel : BaseSearchViewModel
    {
        #region readonlyProperties

        private readonly IAppSettings _appSettings;

        private readonly IMarketPlaceService _marketPlace;

        private readonly IAnalytics _analytics;

        private readonly ILocalDataService _local;

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

        public MainViewModel(ISerializableService serializable, ISearchService search, IAppSettings appSettings, ILocalDataService local, IMarketPlaceService marketPlace, IAnalytics analytics)
        {
            _analytics = analytics;
            _serializable = serializable;
            _search = search;
            _appSettings = appSettings;
            _local = local;
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
        public List<string> VariantOfSearch
        {
            get;
            set;
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
                IsOnDaySelected = SelectedVariant == "На дату" ? true : false;
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
        public async void Init()
        {
            _analytics.SentView("MainView");
            IsDownloadRun = true;
            if (_appSettings.AutoCompletion == null)
                await RestoreData();
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

            List<Train> schedule = await _search.GetTrainSchedule(_appSettings.AutoCompletion.First(x => x.UniqueId == from), _appSettings.AutoCompletion.First(x => x.UniqueId == to), Datum, SelectedVariant);

            if (schedule == null || !schedule.Any())
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["TrainsNotFound"]);
            else
            {
                _appSettings.LastRequestTrain = schedule;
                _appSettings.UpdatedLastRequest = new LastRequest { From = from, To = to, SelectionMode = SelectedVariant, Date = Datum };
                await _serializable.SerializeObjectToXml(_appSettings.UpdatedLastRequest, Constants.UpdateLastRequest);
                await _serializable.SerializeObjectToXml(schedule, Constants.LastTrainList);
                ShowViewModel<ScheduleViewModel>(new { param = JsonConvert.SerializeObject(schedule) });
            }
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
                await Task.Run(() => _serializable.SerializeObjectToXml(Trains, Constants.LastTrainList));
            }

            IsTaskRun = false;
        }

        private void ClickAboutItem(About selectedAboutItem)
        {
            if (selectedAboutItem == null) return;
            if (selectedAboutItem.Item == AboutPicture.AboutApp)
                ShowViewModel<AboutViewModel>();
            else if (selectedAboutItem.Item == AboutPicture.Mail)
                Mvx.Resolve<IMvxComposeEmailTask>().ComposeEmail("sampir.fiesta@gmail.com", string.Empty, "Чыгунка/предложения/баги", String.Empty, false);
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


        private async Task RestoreData()
        {
            VariantOfSearch = new List<string>
                {
                    ResourceLoader.Instance.Resource["Yesterday"],
                    ResourceLoader.Instance.Resource["Today"],
                    ResourceLoader.Instance.Resource["Tommorow"],
                    ResourceLoader.Instance.Resource["OnAllDays"],
                    ResourceLoader.Instance.Resource["OnDay"]
                };
            _appSettings.AutoCompletion = await _local.GetData<List<CountryStopPointItem>>(Constants.StopPointsJson);
            _appSettings.HelpInformation = await _local.GetData<List<HelpInformationItem>>(Constants.HelpInformationJson);
            _appSettings.CarriageModel = await _local.GetData<List<CarriageModel>>(Constants.CarriageModelJson);
            _appSettings.FavoriteRequests = await _serializable.ReadObjectFromXmlFileAsync<List<LastRequest>>(Constants.FavoriteRequests);
            _appSettings.UpdatedLastRequest = await _serializable.ReadObjectFromXmlFileAsync<LastRequest>(Constants.UpdateLastRequest);
            _appSettings.LastRequestTrain = await _serializable.ReadObjectFromXmlFileAsync<List<Train>>(Constants.LastTrainList);
            _appSettings.About = await _local.GetData<List<About>>(Constants.AboutJson);
        }

        #endregion
    }
}
