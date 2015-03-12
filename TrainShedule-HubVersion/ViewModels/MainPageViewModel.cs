using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Email;
using Windows.System;
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

        #endregion

        #region constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="lastRequestTrainService">Used to search deserialize trains from the last request.</param>
        public MainPageViewModel(INavigationService navigationService, ILastRequestTrainService lastRequestTrainService)
        {
            _navigationService = navigationService;
            _lastRequestTrain = lastRequestTrainService;
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
            _navigationService.NavigateToViewModel<EditFavoriteRoutesPageViewModel>();
        }

        /// <summary>
        /// Go to saved by user routes page.
        /// </summary>
        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpPageViewModel>();
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
            _navigationService.NavigateToViewModel<EditFavoriteRoutesPageViewModel>();
        }

        /// <summary>
        /// Invoked when the user pressed on ListBoxItem.
        /// </summary>
        /// <param name="item">Data that describes route.
        /// This parameter is used to transmit the search page trains.</param>
        private void SelectTrain(LastRequest item)
        {
            _navigationService.NavigateToViewModel<ItemPageViewModel>(new LastRequest { From = item.From, To = item.To });
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
            var mail = new EmailMessage {Subject = "Чыгунка/предложения/баги"};

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
            _navigationService.NavigateToViewModel<AboutPageViewModel>();
        }

        #endregion
    }
}

