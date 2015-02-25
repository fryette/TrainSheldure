using System.Threading.Tasks;
using Windows.UI.Xaml;
using Caliburn.Micro;
using TrainShedule_HubVersion.DataModel;
using TrainShedule_HubVersion.Infrastructure;

namespace TrainShedule_HubVersion.ViewModels
{
    public class InformationPageViewModel : Screen
    {
        private readonly INavigationService _navigationService;
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
        protected override void OnActivate()
        {
            AdditionalInformation = Parameter.AdditionalInformation;
        }
        private async void SearchStopPoint()
        {
            if(IsTaskRun)return;
            IsTaskRun = true;
            var stopPointList = await Task.Run(() => TrainStopGrabber.GetTrainStop(Parameter.Link));
            IsTaskRun = false;
            _navigationService.NavigateToViewModel<StopPointPageViewModel>(stopPointList);
        }
    }
}
