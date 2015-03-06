using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Trains.Services.Interfaces;
using TrainSearch.Entities;
using TrainSearch.Infrastructure;

namespace Trains.App.ViewModels
{
    public class MainPageViewModel : Screen
    {
        private readonly IMain _mainService;
        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService, IMain mainService)
        {
            _navigationService = navigationService;
            _mainService = mainService;
        }

        #region properties
        private static IEnumerable<Train> _trains;
        public IEnumerable<Train> Trains
        {
            get { return _trains; }
            set
            {
                _trains = value;
                NotifyOfPropertyChange(() => Trains);
            }
        }

        private IEnumerable<MenuDataItem> _menu;
        public IEnumerable<MenuDataItem> Menu
        {
            get { return _menu; }
            set
            {
                _menu = value;
                NotifyOfPropertyChange(() => Menu);
            }
        }
        #endregion

        #region action
        private void GoToSearch(MenuDataItem item)
        {
            if (item != null && (item.UniqueId == "Menu-Interregional" || item.UniqueId == "Menu-Regional"))
                _navigationService.NavigateToViewModel<SectionPageViewModel>(item);
            else _navigationService.NavigateToViewModel<ItemPageViewModel>(item);
        }

        private void ClickItem(Train train)
        {
            _navigationService.NavigateToViewModel<InformationPageViewModel>(train);
        }

        protected override async void OnActivate()
        {
            Menu = await MenuData.GetItemsAsync();
            Trains = await _mainService.GetTrains();
        }

        private void GoToFavoriteList()
        {
            _navigationService.NavigateToViewModel<FavoritePageViewModel>(Menu.First());
        }

        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpPageViewModel>();
        }
        #endregion
    }
}

