using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;

namespace Trains.App.ViewModels
{
    /// <summary>
    /// Used to dispalying users saved routes.
    /// </summary>
    public class EditFavoriteRoutesViewModel : Screen
    {
        #region constants
        private const string FavoriteStr = "favoriteRequests";
        #endregion

        #region properties

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly IFavoriteManageService _favoriteManage;

        /// <summary>
        /// Used to navigate between pages.
        /// </summary>
        private readonly INavigationService _navigationService;

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
        /// <param name="favoriteManage">Used to manage favorite routes.</param>
        /// <param name="navigationService">Used to navigate between pages.</param>
        public EditFavoriteRoutesViewModel(IFavoriteManageService favoriteManage, INavigationService navigationService)
        {
            _favoriteManage = favoriteManage;
            _navigationService = navigationService;
        }

        #endregion

        #region actions
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        protected override void OnActivate()
        {
            FavoriteRequests = SavedItems.FavoriteRequests;
            ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("NotifyMessage"));
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
        private void DeleteSelectedFavoriteRoutes()
        {
            _favoriteManage.DeleteFavorite(FavoriteRequests);
            if (!SavedItems.FavoriteRequests.Any())
                _navigationService.NavigateToViewModel<MainViewModel>();
            FavoriteRequests = SavedItems.FavoriteRequests;
        }
        #endregion
    }
}
