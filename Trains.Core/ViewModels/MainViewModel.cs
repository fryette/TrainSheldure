using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Trains.Entities;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;

namespace Trains.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        #region readonlyProperties
        //<summary>
        //Used to get trains from the last request.
        //</summary>
        private readonly ILastRequestTrainService _lastRequestTrainService;

        private readonly IStartService _start;

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
        public IMvxCommand GoToNewsCommand { get; private set; }
        public IMvxCommand GoToFavoriteCommand { get; private set; }
        public IMvxCommand GoToHelpCommand { get; private set; }
        public IMvxCommand GoToMarketPlaceCommand { get; private set; }
        public IMvxCommand GoToAboutPageCommand { get; private set; }
        public IMvxCommand GoToSettingsPageCommand { get; private set; }
        public IMvxCommand ClickItemCommand { get; private set; }
        public IMvxCommand SelectTrainCommand { get; private set; }
        public IMvxCommand SentEmailCommand { get; private set; }
        public IMvxCommand UpdateLastRequestCommand { get; private set; }

        #endregion

        #region ctor

        public MainViewModel(ILastRequestTrainService lastRequestTrainService, IStartService start, ISerializableService serializable, ISearchService search)
        {
            _lastRequestTrainService = lastRequestTrainService;
            _start = start;
            _serializable = serializable;
            _search = search;

            GoToSearchCommand = new MvxCommand(GoToSearch);
            GoToFavoriteCommand = new MvxCommand(GoToFavorite);
            GoToFavoriteListCommand = new MvxCommand(GoToFavoriteList);
            GoToNewsCommand = new MvxCommand(GoToNews);
            GoToHelpCommand = new MvxCommand(GoToHelpPage);
            GoToMarketPlaceCommand = new MvxCommand(GoToMarketPlace);
            GoToAboutPageCommand = new MvxCommand(GoToAboutPage);
            GoToSettingsPageCommand = new MvxCommand(GoToSettingsPage);
            ClickItemCommand = new MvxCommand(() => ClickItem(SelectedTrain));
            SelectTrainCommand = new MvxCommand(() => SelectTrain(LastRequestTrain));
            SentEmailCommand = new MvxCommand(SentEmail);
            UpdateLastRequestCommand = new MvxCommand(UpdateLastRequest);
        }

        #endregion

        #region properties

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

        private LastRequest _lastRequestTrain;
        public LastRequest LastRequestTrain
        {
            get { return _lastRequestTrain; }
            set
            {
                _lastRequestTrain = value;
                RaisePropertyChanged(() => LastRequestTrain);
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
            IsDownloadRun = true;
            await _start.RestoreData();
            IsBarDownloaded = true;
            if (SavedItems.UpdatedLastRequest != null)
                LastRoute = String.Format("{0} - {1}", SavedItems.UpdatedLastRequest.From, SavedItems.UpdatedLastRequest.To);
            Trains = await _lastRequestTrainService.GetTrains();
            FavoriteRequests = SavedItems.FavoriteRequests;
            IsDownloadRun = false;
        }

        /// <summary>
        /// Go to page,where user must enter the stopping points.
        /// </summary>
        private void GoToSearch()
        {
            ShowViewModel<ItemViewModel>();
        }

        /// <summary>
        /// Invoked when the user selects his train of interest.
        /// </summary>
        /// <param name="train">Data that describes user-selected train(prices,seats,stop points,and other)</param>
        private void ClickItem(Train train)
        {
            ShowViewModel<InformationViewModel>(train);
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

        private async void GoToNews()
        {
            //await LaunchUriAsync(new Uri("https://vk.com/belrailway"));
        }

        /// <summary>
        /// Used to manage user-saved routes.
        /// </summary>
        private void GoToFavorite()
        {
            if (SavedItems.FavoriteRequests == null || !SavedItems.FavoriteRequests.Any()) ;
            //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("EditFavoriteMessageError"));
            else
                ShowViewModel<EditFavoriteRoutesViewModel>();

        }

        /// <summary>
        /// Invoked when the user pressed on ListBoxItem.
        /// </summary>
        /// <param name="item">Data that describes route.
        /// This parameter is used to transmit the search page trains.</param>
        private void SelectTrain(LastRequest item)
        {
            ShowViewModel<ItemViewModel>(new LastRequest { From = item.From, To = item.To });
        }

        /// <summary>
        /// Used to sent email to sampir.fiesta@gmail.com,or whant retain a comment about this App.
        /// </summary>
        private async void SentEmail()
        {
            ////predefine Recipient
            //var sendTo = new EmailRecipient
            //{
            //    Address = "sampir.fiesta@gmail.com"
            //};

            ////generate mail object
            //var mail = new EmailMessage { Subject = "Чыгунка/предложения/баги" };

            ////add recipients to the mail object
            //mail.To.Add(sendTo);

            //await EmailManager.ShowComposeNewEmailAsync(mail);
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
            IsTaskRun = true;
            var trains = await Task.Run(() => _search.UpdateTrainSchedule());
            if (trains == null) ;
            //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("InternetConnectionError"));
            else
            {
                Trains = trains;
                await Task.Run(() => _serializable.SerializeObjectToXml(Trains, "LastTrainList"));
            }
            IsTaskRun = false;
        }

        private void GoToSettingsPage()
        {
            ShowViewModel<SettingsViewModel>();
        }

        #endregion
    }
}
