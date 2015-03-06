using System.Collections.Generic;
using Caliburn.Micro;
using Trains.Services.Interfaces;
using TrainSearch.Entities;

namespace Trains.App.ViewModels
{
    public class FavoritePageViewModel : Screen
    {
        private readonly INavigationService _navigationService;
        private readonly ISerializable _serializable;
        
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
        public FavoritePageViewModel(INavigationService navigationService,ISerializable serializable)
        {
            _navigationService = navigationService;
            _serializable = serializable;
        }

        protected override async void OnActivate()
        {
            FavoriteRequests = await _serializable.GetLastRequests("favoriteRequests");
        }

        private void SelectItem(LastRequest item)
        {
            Parameter.From = item.From;
            Parameter.To = item.To;
            _navigationService.NavigateToViewModel<ItemPageViewModel>(Parameter);
        }

        private void DeleteAllFavorite()
        {
            _serializable.DeleteFile("favoriteRequests");
            _navigationService.NavigateToViewModel<ItemPageViewModel>(Parameter);
        }
    }
}
