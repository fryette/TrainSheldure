using System.Collections.Generic;
using Caliburn.Micro;
using TrainShedule_HubVersion.DataModel;

namespace TrainShedule_HubVersion.ViewModels
{
    public class StopPointPageViewModel : Screen
    {
        public IEnumerable<TrainStop> Parameter { get; set; }

        private readonly INavigationService _navigationService;
        private readonly ILog _log;
        public StopPointPageViewModel(INavigationService navigationService, ILog log)
        {
            _navigationService = navigationService;
            _log = log;
        }
        protected override void OnActivate()
        {
        }
    }
}
