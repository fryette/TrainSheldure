using System.Collections.Generic;
using Caliburn.Micro;
using TrainSearch.Entities;

namespace Trains.App.ViewModels
{
    public class StopPointPageViewModel : Screen
    {
        private readonly INavigationService _navigationService;
        public IEnumerable<TrainStop> Parameter { get; set; }
        protected override void OnActivate() { }

        public StopPointPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private void GoToHomePage()
        {
            _navigationService.NavigateToViewModel<MainPageViewModel>();
        }

        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpPageViewModel>();
        }
    }

}
