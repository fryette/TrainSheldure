using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Caliburn.Micro;
using TrainSearch.Entities;
using TrainSearch.Infrastructure;

namespace TrainShedule_HubVersion.ViewModels
{
    public class InformationPageViewModel : Screen
    {

        private readonly INavigationService _navigationService;
        #region properties
        public Train Parameter { get; set; }
        public InformationPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

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
        protected override void OnActivate()
        {
            AdditionalInformation = Parameter.AdditionalInformation;
        }
        private async void SearchStopPoint()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (IsTaskRun) return;
                IsTaskRun = true;
                var stopPointList = await Task.Run(() => TrainStopGrabber.GetTrainStop(Parameter.Link));
                IsTaskRun = false;
                _navigationService.NavigateToViewModel<StopPointPageViewModel>(stopPointList);
            }
            else
            {
                var messageDialog = new MessageDialog("Доступ к интернету отсутствует,проверьте подключение!");
                await messageDialog.ShowAsync();
            }
        }
        #endregion
    }
}