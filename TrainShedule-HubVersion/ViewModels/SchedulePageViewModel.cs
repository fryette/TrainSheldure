using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Trains.Services.Interfaces;
using TrainSearch.Entities;

namespace Trains.App.ViewModels
{
    public class SchedulePageViewModel : Screen
    {
        public IEnumerable<Train> Parameter { get; set; }
        private readonly INavigationService _navigationService;
        private readonly ISerializable _serializable;

        public SchedulePageViewModel(INavigationService navigationService, ISerializable serializable)
        {
            _navigationService = navigationService;
            _serializable = serializable;
        }
        #region action
        protected override void OnActivate()
        {
            _serializable.SerializeObjectToXml(Parameter.ToList(), "LastTrainList");
        }

        private void ClickItem(Train train)
        {
            _navigationService.NavigateToViewModel<InformationPageViewModel>(train);
        }
        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpPageViewModel>();
        }
        private void GoToHomePage()
        {
            _navigationService.NavigateToViewModel<MainPageViewModel>();
        }
        #endregion
    }
}
