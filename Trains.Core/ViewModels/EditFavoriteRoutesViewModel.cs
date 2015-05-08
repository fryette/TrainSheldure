using System.Collections.Generic;
using System.Linq;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class EditFavoriteRoutesViewModel : MvxViewModel
    {
        #region readonlyProperties

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly IFavoriteManageService _favoriteManage;

        private readonly IAppSettings _appSettings;

        #endregion

        #region command

        public IMvxCommand DeleteCommand { get; private set; }
        public MvxCommand<LastRequest> SelectItemCommand { get; private set; }

        #endregion

        #region ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="favoriteManage">Used to manage favorite routes.</param>
        /// <param name="appSettings"></param>
        public EditFavoriteRoutesViewModel(IFavoriteManageService favoriteManage, IAppSettings appSettings)
        {
            _favoriteManage = favoriteManage;
            _appSettings = appSettings;

            DeleteCommand = new MvxCommand(DeleteSelectedFavoriteRoutes);
            SelectItemCommand = new MvxCommand<LastRequest>(SelectItem);
        }

        #endregion

        #region properties

        #region UIproperties

        public string EditFavoriteHeader { get; set; }
        public string DeleteAppBar { get; set; }

        #endregion

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
                RaisePropertyChanged(() => FavoriteRequests);
            }
        }

        #endregion

        #region actions
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        public async void Init()
        {
            RestoreUIBindings();
            FavoriteRequests = _appSettings.FavoriteRequests;
            await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["NotifyMessage"]);
        }

        /// <summary>
        /// Invoked when the user pressed on ListBoxItem.
        /// </summary>
        /// <param name="selectedRoute"></param>
        private void SelectItem(LastRequest selectedRoute)
        {
            if (selectedRoute == null) return;
            selectedRoute.IsCanBeDeleted = !selectedRoute.IsCanBeDeleted;
            //TODO remove and ask how create NotifyChangeProp
            FavoriteRequests = FavoriteRequests.Select(x => x).ToList();
        }

        /// <summary>
        /// Deleted all all favorite saved routes.
        /// </summary>
        private void DeleteSelectedFavoriteRoutes()
        {
            _favoriteManage.ManageFavorite(FavoriteRequests);
            if (!_appSettings.FavoriteRequests.Any())
                ShowViewModel<MainViewModel>();
            FavoriteRequests = _appSettings.FavoriteRequests;
        }

        private void RestoreUIBindings()
        {
            EditFavoriteHeader = ResourceLoader.Instance.Resource["EditFavoriteHeader"];
            DeleteAppBar = ResourceLoader.Instance.Resource["DeleteAppBar"];
        }

    }

        #endregion
}