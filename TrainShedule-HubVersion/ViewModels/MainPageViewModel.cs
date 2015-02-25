using System.Collections.Generic;
using Caliburn.Micro;
using TrainShedule_HubVersion.DataModel;
using TrainShedule_HubVersion.Infrastructure;

namespace TrainShedule_HubVersion.ViewModels
{
    public class MainPageViewModel : Screen
    {
        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

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

        private static IEnumerable<MenuDataItem> _menu;
        public IEnumerable<MenuDataItem> Menu
        {
            get { return _menu; }
            set
            {
                _menu = value;
                NotifyOfPropertyChange(() => Menu);
            }
        }

        public void GoToSearch(MenuDataItem item)
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
            if (_menu != null && _trains != null) return;
            Menu = await MenuData.GetItemsAsync();
            Trains = await Serialize.ReadObjectFromXmlFileAsync<Train>("LastTrainList");
        }
    }
}

