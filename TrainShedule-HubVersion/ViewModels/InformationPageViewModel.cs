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
        private readonly ILog _log;
        public Train Parameter { get; set; }
        public InformationPageViewModel(INavigationService navigationService, ILog log)
        {
            _navigationService = navigationService;
            _log = log;
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
        private Visibility _visibilityProgressBar;
        public Visibility VisibilityProgressBar
        {
            get { return _visibilityProgressBar; }
            set
            {
                _visibilityProgressBar = value;
                NotifyOfPropertyChange(() => VisibilityProgressBar);
            }
        }
        protected override void OnActivate()
        {
            VisibilityProgressBar = Visibility.Collapsed;
            AdditionalInformation = Parameter.AdditionalInformation;
        }
        private async void SearchStopPoint()
        {
            VisibilityProgressBar = Visibility.Visible;
            var stopPointList = await Task.Run(() => AllTrainStop.GetTrainStop(Parameter.City.Substring(0, Parameter.City.IndexOf(" ", System.StringComparison.Ordinal)), Parameter.DepartureDate));
            VisibilityProgressBar = Visibility.Collapsed;
            _navigationService.NavigateToViewModel<StopPointPageViewModel>(stopPointList);
        }
    }
}
