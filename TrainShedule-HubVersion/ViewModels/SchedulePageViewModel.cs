using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using TrainShedule_HubVersion.Infrastructure;

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
        #region action
        protected override void OnActivate()
        {
            Task.Run(() => Serialize.SaveObjectToXml(new List<Train>(Parameter), "LastTrainList"));
        }

        private void ClickItem(Train train)
        {
            _navigationService.NavigateToViewModel<InformationPageViewModel>(train);
        }
        #endregion
    }
}
