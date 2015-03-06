using System.Collections.Generic;
using Caliburn.Micro;
using TrainSearch.Entities;
using TrainSearch.Infrastructure;

namespace Trains.App.ViewModels
{
    public class HelpPageViewModel : Screen
    {
        private static IEnumerable<MenuDataItem> _menu;
        private INavigationService _navigationService;

        public IEnumerable<MenuDataItem> Menu
        {
            get { return _menu; }
            set
            {
                _menu = value;
                NotifyOfPropertyChange(() => Menu);
            }
        }

        public HelpPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        protected override async void OnActivate()
        {
            Menu = await MenuData.GetItemsAsync();
        }

        private void GoToHomePage()
        {
            _navigationService.NavigateToViewModel<MainPageViewModel>();
        }
    }
}
