using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.System;
using Caliburn.Micro;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;
using Trains.Entities;

namespace Trains.App.ViewModels
{
    /// <summary>
    /// Model by start page
    /// </summary>
    public class MainViewModel : Screen
    {
        #region constants

        private const string EditFavoriteMessageError =
            "Сохраните хотя бы одну станцию, что бы иметь возможность редактирования";
        #endregion
        #region readonlyProperties
        /// <summary>
        /// Used to get trains from the last request.
        /// </summary>
        private readonly ILastRequestTrainService _lastRequestTrain;


        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly ISerializableService _serializable;

        /// <summary>
        /// Used to navigate between pages.
        /// </summary>
        private readonly INavigationService _navigationService;

        /// <summary>
        /// Used to search train schedule.
        /// </summary>
        private readonly ISearchService _search;

        #endregion

        #region constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="lastRequestTrainService">Used to search deserialize trains from the last request.</param>
        /// <param name="search"></param>
        public MainViewModel(INavigationService navigationService, ILastRequestTrainService lastRequestTrainService, ISearchService search, ISerializableService serializable)
        {
            _navigationService = navigationService;
            _lastRequestTrain = lastRequestTrainService;
            _search = search;
            _serializable = serializable;
        }
        #endregion

        #region properties

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
                NotifyOfPropertyChange(() => IsTaskRun);
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
                NotifyOfPropertyChange(() => Trains);
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
                NotifyOfPropertyChange(() => FavoriteRequests);
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
            Trains = await _lastRequestTrain.GetTrains();
            FavoriteRequests = SavedItems.FavoriteRequests;
        }

        /// <summary>
        /// Go to page,where user must enter the stopping points.
        /// </summary>
        private void GoToSearch()
        {
            _navigationService.NavigateToViewModel<ItemViewModel>();
        }

        /// <summary>
        /// Invoked when the user selects his train of interest.
        /// </summary>
        /// <param name="train">Data that describes user-selected train(prices,seats,stop points,and other)</param>
        private void ClickItem(Train train)
        {
            _navigationService.NavigateToViewModel<InformationViewModel>(train);
        }
        /// <summary>
        /// Go to favorite routes page.
        /// </summary>
        private void GoToFavoriteList()
        {
            _navigationService.NavigateToViewModel<EditFavoriteRoutesViewModel>();
        }

        /// <summary>
        /// Go to saved by user routes page.
        /// </summary>
        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpViewModel>();
        }

        private async void GoToNews()
        {
            await Launcher.LaunchUriAsync(new Uri("https://vk.com/belrailway"));
        }

        /// <summary>
        /// Used to manage user-saved routes.
        /// </summary>
        private void GoToFavorite()
        {
            if (!SavedItems.FavoriteRequests.Any()) ToolHelper.ShowMessageBox(EditFavoriteMessageError);
            else
                _navigationService.NavigateToViewModel<EditFavoriteRoutesViewModel>();
        }

        /// <summary>
        /// Invoked when the user pressed on ListBoxItem.
        /// </summary>
        /// <param name="item">Data that describes route.
        /// This parameter is used to transmit the search page trains.</param>
        private void SelectTrain(LastRequest item)
        {
            _navigationService.NavigateToViewModel<ItemViewModel>(new LastRequest { From = item.From, To = item.To });
        }

        /// <summary>
        /// Used to sent email to sampir.fiesta@gmail.com,or whant retain a comment about this App.
        /// </summary>
        private async void SentEmail()
        {
            //predefine Recipient
            var sendTo = new EmailRecipient()
            {
                Address = "sampir.fiesta@gmail.com"
            };

            //generate mail object
            var mail = new EmailMessage { Subject = "Чыгунка/предложения/баги" };

            //add recipients to the mail object
            mail.To.Add(sendTo);

            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        /// <summary>
        /// Used to evaluate the application in the Windows phone store.
        /// </summary>
        private async void GoToMarketPlace()
        {
            var uri = new Uri("ms-windows-store:reviewapp?appid=9a0879a6-0764-4e99-87d7-4c1c33f2d78e");
            await Launcher.LaunchUriAsync(uri);
        }

        /// <summary>
        /// Used to read information about this application.
        /// </summary>
        private void GoToAboutPage()
        {
            _navigationService.NavigateToViewModel<AboutViewModel>();
        }

        /// <summary>
        /// Update last route
        /// </summary>
        private async void UpdateLastRequest()
        {
            if (SavedItems.UpdatedLastRequest == null) ToolHelper.ShowMessageBox("Не найдет последний запрос,или произошла ошибка загрузки");
            if (IsTaskRun) return;
            IsTaskRun = true;
            var trains =
                await Task.Run(() => _search.GetTrainSchedule(SavedItems.UpdatedLastRequest.From,
                                SavedItems.UpdatedLastRequest.To, ToolHelper.GetDate(DateTime.Now)));
            IsTaskRun = false;
            if (trains == null)
            {
                ToolHelper.ShowMessageBox("Обновление не удалось,проверьте подключение к интернету и попробуйте сного");
                return;
            }
            Trains = trains;
            await Task.Run(() => _serializable.SerializeObjectToXml(Trains, "LastTrainList"));
        }
        #endregion
    }
}

