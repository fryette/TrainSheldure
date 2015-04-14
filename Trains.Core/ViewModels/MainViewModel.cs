﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Trains.Entities;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Newtonsoft.Json;
using System.Reflection;
using System.Resources;
using Trains.Resources;
using Trains.Core.Interfaces;
using Cirrious.MvvmCross.Plugins.Email;
using System.Runtime.ExceptionServices;

namespace Trains.Core.ViewModels
{
    public class MainViewModel : BaseSearchViewModel
    {
        #region readonlyProperties

        private readonly IAppSettings _appSettings;

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

        public IMvxCommand GoToSearchCommand { get; private set; }
        public IMvxCommand GoToFavoriteListCommand { get; private set; }
        public IMvxCommand GoToFavoriteCommand { get; private set; }
        public IMvxCommand GoToHelpCommand { get; private set; }
        public IMvxCommand GoToMarketPlaceCommand { get; private set; }
        public IMvxCommand GoToAboutPageCommand { get; private set; }
        public IMvxCommand GoToSettingsPageCommand { get; private set; }
        public IMvxCommand ClickItemCommand { get; private set; }
        public IMvxCommand SelectFavoriteTrainCommand { get; private set; }
        public IMvxCommand SentEmailCommand { get; private set; }
        public IMvxCommand UpdateLastRequestCommand { get; private set; }
        public IMvxCommand SearchCommand { get; private set; }


        #endregion

        #region ctor

        public MainViewModel(ISerializableService serializable, ISearchService search, IAppSettings appSettings, ILocalDataService local)
        {
            _serializable = serializable;
            _search = search;
            _appSettings = appSettings;
            _local = local;

            GoToSearchCommand = new MvxCommand(GoToSearch);
            GoToFavoriteCommand = new MvxCommand(GoToFavorite);
            GoToFavoriteListCommand = new MvxCommand(GoToFavoriteList);
            GoToHelpCommand = new MvxCommand(GoToHelpPage);
            GoToMarketPlaceCommand = new MvxCommand(GoToMarketPlace);
            GoToAboutPageCommand = new MvxCommand(GoToAboutPage);
            GoToSettingsPageCommand = new MvxCommand(GoToSettingsPage);
            ClickItemCommand = new MvxCommand(() => ClickItem(SelectedTrain));
            SelectFavoriteTrainCommand = new MvxCommand(() => SelectFavoriteTrain(SelectedRoute));
            SentEmailCommand = new MvxCommand(SentEmail);
            UpdateLastRequestCommand = new MvxCommand(UpdateLastRequest);
            SearchCommand = new MvxCommand(Search);
        }

        #endregion

        #region properties

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
            get
            {
                return new List<string>
                {
                    "Вчера","Сегодня","Завтра","Все дни","На дату"
                };
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

        private Train _selectedTrain;

        public Train SelectedTrain
        {
            get { return _selectedTrain; }
            set
            {
                _selectedTrain = value;
                RaisePropertyChanged(() => SelectedTrain);
            }
        }

        private LastRequest _selectedRoute;

        public LastRequest SelectedRoute
        {
            get { return _selectedRoute; }
            set
            {
                _selectedRoute = value;
                RaisePropertyChanged(() => SelectedRoute);
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

        #endregion

        #region actions

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        public async void Init()
        {
            SelectedVariant = VariantOfSearch[1];
            IsDownloadRun = true;
            if (_appSettings.AutoCompletion == null)
                await RestoreData();
            IsBarDownloaded = true;
            if (_appSettings.UpdatedLastRequest != null)
                LastRoute = String.Format("{0} - {1}", _appSettings.UpdatedLastRequest.From, _appSettings.UpdatedLastRequest.To);
            Trains = _appSettings.LastRequestTrain;
            FavoriteRequests = _appSettings.FavoriteRequests;
            IsDownloadRun = false;
        }

        /// <summary>
        /// Searches for train schedules at a specified date in the specified mode.
        /// </summary>
        private async void Search()
        {
            if (IsTaskRun || await CheckInput(Datum, From, To, _appSettings.AutoCompletion)) return;
            IsTaskRun = true;
            List<Train> schedule = await _search.GetTrainSchedule(_appSettings.AutoCompletion.First(x => x.UniqueId == From), _appSettings.AutoCompletion.First(x => x.UniqueId == To), Datum, SelectedVariant);

            if (schedule == null || !schedule.Any())
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("TrainsNotFound"));
            else
            {
                _appSettings.LastRequestTrain = schedule;
                await SerializeDataSearch();
                await _serializable.SerializeObjectToXml(schedule, Constants.LastTrainList);
                ShowViewModel<ScheduleViewModel>(new { param = JsonConvert.SerializeObject(schedule) });
            }
            IsTaskRun = false;
        }

        private async Task SerializeDataSearch()
        {
            _appSettings.UpdatedLastRequest = new LastRequest { From = From, To = To, SelectionMode = SelectedVariant, Date = Datum };
            await _serializable.SerializeObjectToXml(_appSettings.UpdatedLastRequest, Constants.UpdateLastRequest);
        }

        /// <summary>
        /// Go to page,where user must enter the stopping points.
        /// </summary>
        private void GoToSearch()
        {
            ShowViewModel<SearchViewModel>();
        }

        /// <summary>
        /// Invoked when the user selects his train of interest.
        /// </summary>
        /// <param name="train">Data that describes user-selected train(prices,seats,stop points,and other)</param>
        private void ClickItem(Train train)
        {
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
        private async void GoToFavorite()
        {
            if (_appSettings.FavoriteRequests == null || !_appSettings.FavoriteRequests.Any())
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("EditFavoriteMessageError"));
            else
                ShowViewModel<EditFavoriteRoutesViewModel>();
        }

        /// <summary>
        /// Invoked when the user pressed on ListBoxItem.
        /// </summary>
        /// <param name="item">Data that describes route.
        /// This parameter is used to transmit the search page trains.</param>
        private void SelectFavoriteTrain(LastRequest item)
        {
            ShowViewModel<SearchViewModel>(new { param = JsonConvert.SerializeObject(item) });
        }

        /// <summary>
        /// Used to sent email to sampir.fiesta@gmail.com,or whant retain a comment about this App.
        /// </summary>
        private void SentEmail()
        {
            Mvx.Resolve<IMvxComposeEmailTask>().ComposeEmail("sampir.fiesta@gmail.com",
                  string.Empty,
                  "Чыгунка/предложения/баги",
                  String.Empty,
                  false);
        }

        /// <summary>
        /// Used to evaluate the application in the Windows phone store.
        /// </summary>
        private async void GoToMarketPlace()
        {
            //var uri = new Uri("ms-windows-store:reviewapp?appid=9a0879a6-0764-4e99-87d7-4c1c33f2d78e");
            //await Launcher.LaunchUriAsync(uri);
        }

        /// <summary>
        /// Used to read information about this application.
        /// </summary>
        private void GoToAboutPage()
        {
            ShowViewModel<AboutViewModel>();
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
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("InternetConnectionError"));
            else
            {
                Trains = trains;
                await Task.Run(() => _serializable.SerializeObjectToXml(Trains, Constants.LastTrainList));
            }

            IsTaskRun = false;
        }

        private void GoToSettingsPage()
        {
            ShowViewModel<SettingsViewModel>();
        }

        private async Task RestoreData()
        {
            _appSettings.AutoCompletion = (await _local.GetData<List<CountryStopPointItem>>(Constants.StopPointsJson)).ToList();
            _appSettings.HelpInformation = (await _local.GetData<List<HelpInformationItem>>(Constants.HelpInformationJson));
            _appSettings.FavoriteRequests = await _serializable.ReadObjectFromXmlFileAsync<List<LastRequest>>(Constants.FavoriteRequests);
            _appSettings.UpdatedLastRequest = await _serializable.ReadObjectFromXmlFileAsync<LastRequest>(Constants.UpdateLastRequest);
            _appSettings.LastRequestTrain = await _serializable.ReadObjectFromXmlFileAsync<List<Train>>(Constants.LastTrainList);
            _appSettings.LastRequests = await _serializable.ReadObjectFromXmlFileAsync<List<LastRequest>>(Constants.LastRequests);
            _appSettings.CarriageModel = (await _local.GetData<List<CarriageModel>>(Constants.CarriageModelJson));
        }

        #endregion
    }
}
