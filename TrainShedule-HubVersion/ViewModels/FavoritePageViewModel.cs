using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.App.ViewModels
{
    /// <summary>
    /// Used to dispalying users saved routes.
    /// </summary>
    public class FavoritePageViewModel : Screen
    {
        /// <summary>
        /// Used to navigate between pages.
        /// </summary>
        private readonly INavigationService _navigationService;

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly ISerializableService _serializable;

        /// <summary>
        /// Object are stored custom routes.
        /// </summary>
        private List<LastRequest> _favoriteRequests;
        public List<LastRequest> FavoriteRequests
        {
            get { return _favoriteRequests; }
            set
            {
                _favoriteRequests = value;
                NotifyOfPropertyChange(() => FavoriteRequests);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        public FavoritePageViewModel(INavigationService navigationService, ISerializableService serializable)
        {
            _navigationService = navigationService;
            _serializable = serializable;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        protected override void OnActivate()
        {
            FavoriteRequests = SavedItems.FavoriteRequests;
        }

        /// <summary>
        /// Invoked when the user pressed on ListBoxItem.
        /// </summary>
        /// <param name="item">Data that describes route.
        /// This parameter is used to transmit the search page trains.</param>
        private void SelectItem(LastRequest item)
        {
            item.IsCanBeDeleted = !item.IsCanBeDeleted;
            //TODO remove and ask how create NotifyChangeProp
            FavoriteRequests = FavoriteRequests.Select(x => x).ToList();
        }

        /// <summary>
        /// Deleted all all favorite saved routes.
        /// </summary>
        private void DeleteAllFavorite()
        {
            foreach (var lastRequest in FavoriteRequests.Where(x => x.IsCanBeDeleted))
            {
                SavedItems.FavoriteRequests.Remove(lastRequest);
            }
            FavoriteRequests = SavedItems.FavoriteRequests;
            _serializable.SerializeObjectToXml(SavedItems.FavoriteRequests, "favoriteRequests");
            //_navigationService.NavigateToViewModel<ItemPageViewModel>();
        }
    }
}
