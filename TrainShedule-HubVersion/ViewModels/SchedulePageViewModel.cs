using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using TrainShedule_HubVersion.DataModel;
using TrainShedule_HubVersion.Infrastructure;

namespace TrainShedule_HubVersion.ViewModels
{
  public class SchedulePageViewModel : Screen
    {
        public IEnumerable<Train> Parameter { get; set; }
        private readonly INavigationService _navigationService;
        private readonly ILog _log;
        public SchedulePageViewModel(INavigationService navigationService, ILog log)
        {
            _navigationService = navigationService;
            _log = log;
        }

        private IEnumerable<Train> _trains;
        public IEnumerable<Train> Trains
        {
            get { return _trains; }
            set
            {
                _trains = value;
                NotifyOfPropertyChange(() => Trains);
            }
        }

        protected override void OnActivate()
        {
            Trains = Parameter.Where(x => !x.BeforeDepartureTime.Contains('-'));
            Task.Run(() => Serialize.SaveObjectToXml(new List<Train>(Parameter), "LastTrainList"));
        }

        private void ClickItem(Train train)
        {
            _navigationService.NavigateToViewModel<InformationPageViewModel>(train);
        }
    }
}
