using System;
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using Caliburn.Micro;
using Trains.Services.Interfaces;
using TrainSearch.Entities;

namespace Trains.App.ViewModels
{
    /// <summary>
    /// Used to displaying stop points on selected route.
    /// </summary>
    public class InformationViewModel : Screen
    {
        #region constant
        
        private const string InternetConnectionError = "Доступ к интернету отсутствует,проверьте подключение!";

        #endregion

        #region readonlyProperties
        /// <summary>
        /// Used to navigate between pages.
        /// </summary>
        private readonly INavigationService _navigationService;
        /// <summary>
        /// Used to grab train stops.
        /// </summary>
        private readonly ITrainStopService _trainStop;

        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="trainStop">Used to grab train stops.</param>
        public InformationViewModel(INavigationService navigationService, ITrainStopService trainStop)
        {
            _navigationService = navigationService;
            _trainStop = trainStop;
        }

        #endregion

        #region properties

        /// <summary>
        /// User-selected train.
        /// </summary>
        public Train Parameter { get; set; }

        /// <summary>
        /// User-selected train.
        /// Used to save train if user whant to search TrainStops
        /// </summary>        
        private static Train SavedLasTrainInformations { get; set; }

        /// <summary>
        /// Used to dispalying informations about the seats and their prices.
        /// </summary>
        private AdditionalInformation[] _additionalInformation;
        public AdditionalInformation[] AdditionalInformation
        {
            get { return _additionalInformation; }
            set
            {
                _additionalInformation = value;
                NotifyOfPropertyChange(() => AdditionalInformation);
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
                NotifyOfPropertyChange(() => IsTaskRun);
            }
        }

        #endregion

        #region action

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the additional informations about user-selected train.
        /// </summary>
        protected override void OnActivate()
        {
            if (Parameter == null)
                Parameter = SavedLasTrainInformations;
            else
                AdditionalInformation = Parameter.AdditionalInformation;
        }

        /// <summary>
        /// Search additional information about user-selected train.
        /// </summary>
        private async void SearchStopPoint()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (IsTaskRun) return;
                IsTaskRun = true;
                var stopPointList = await _trainStop.GetTrainStop(Parameter.Link);
                IsTaskRun = false;
                SavedLasTrainInformations = Parameter;
                _navigationService.NavigateToViewModel<StopPointViewModel>(stopPointList);
            }
            else
            {
                var messageDialog = new MessageDialog(InternetConnectionError);
                await messageDialog.ShowAsync();
            }
        }

        #endregion
    }
}