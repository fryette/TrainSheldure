using System.Collections.Generic;
using Caliburn.Micro;
using TrainShedule_HubVersion.Entities;
using TrainShedule_HubVersion.Infrastructure;

namespace TrainShedule_HubVersion.ViewModels
{
    public class FavoritePageViewModel : Screen
    {
        private readonly INavigationService _navigationService;
        public MenuDataItem Parameter { get; set; }

        private IEnumerable<LastRequest> _favoriteRequests;

        public IEnumerable<LastRequest> FavoriteRequests
        {
            get { return _favoriteRequests; }
            set
            {
                _favoriteRequests = value;
                NotifyOfPropertyChange(() => FavoriteRequests);
            }
        }
        public FavoritePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        protected override async void OnActivate()
        {
            FavoriteRequests = await Serialize.ReadObjectFromXmlFileAsync<LastRequest>("favoriteRequests");
        }

        private void SelectItem(LastRequest item)
        {
            Parameter.From = item.From;
            Parameter.To = item.To;
            _navigationService.NavigateToViewModel<ItemPageViewModel>(Parameter);
        }
    }
}
