using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using System.Linq;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Trains.Core.Interfaces;

namespace Trains.Core.ViewModels
{
    public class EditFavoriteRoutesViewModel : MvxViewModel
    {
        #region properties

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly IFavoriteManageService _favoriteManage;

        private readonly IAppSettings _appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="favoriteManage">Used to manage favorite routes.</param>
        public EditFavoriteRoutesViewModel(IFavoriteManageService favoriteManage, IAppSettings appSettings)
        {
            _favoriteManage = favoriteManage;
            _appSettings = appSettings;
        }

        #endregion

        #region command

        public IMvxCommand DeleteCommand { get; private set; }
        public IMvxCommand SelectItemCommand { get; private set; }

        #endregion

        #region ctor

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
            FavoriteRequests = _appSettings.FavoriteRequests;
            DeleteCommand = new MvxCommand(DeleteSelectedFavoriteRoutes);
            SelectItemCommand = new MvxCommand(() => SelectItem(SelectedItem));
            await Mvx.Resolve<IUserInteraction>().AlertAsync(_appSettings.Resource.GetString("NotifyMessage"));
        }


        private LastRequest _selectedItem;
        public LastRequest SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
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
            _favoriteManage.ManageFavorite(FavoriteRequests);
            if (!_appSettings.FavoriteRequests.Any())
                ShowViewModel<MainViewModel>();
            FavoriteRequests = _appSettings.FavoriteRequests;
        }
    }
        #endregion
}