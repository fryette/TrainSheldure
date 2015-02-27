using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using TrainShedule_HubVersion.Entities;
using TrainShedule_HubVersion.Entities;

namespace TrainShedule_HubVersion.ViewModels
{
    public class SchedulePageViewModel : Screen
    {
        public IEnumerable<Train> Parameter { get; set; }
        private readonly INavigationService _navigationService;

        public SchedulePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
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
            Trains = Parameter.Where(x => x.BeforeDepartureTime == null || !x.BeforeDepartureTime.Contains('-'));
            Task.Run(() => Serialize.SaveObjectToXml(new List<Train>(Trains), "LastTrainList"));
        }

        private void ClickItem(Train train)
        {
            _navigationService.NavigateToViewModel<InformationPageViewModel>(train);
        }
    }
}
