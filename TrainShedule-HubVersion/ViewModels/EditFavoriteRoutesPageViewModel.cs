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
    public class EditFavoriteRoutesPageViewModel : Screen
    {
        #region constants
        private const string NotifyMessage = "Выберите интересующие вас станции,затем выберите кнопку удалить выбранные";
        #endregion

        #region properties

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
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        public EditFavoriteRoutesPageViewModel(ISerializableService serializable)
        {
            _serializable = serializable;
        }

        #endregion

        #region actions
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        protected override void OnActivate()
        {
            FavoriteRequests = SavedItems.FavoriteRequests;
            ToolHelper.ShowMessageBox(NotifyMessage);
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
        private void DeleteSelectedFavoritsRoutes()
        {
            foreach (var lastRequest in FavoriteRequests.Where(x => x.IsCanBeDeleted))
            {
                SavedItems.FavoriteRequests.Remove(lastRequest);
            }
            FavoriteRequests = SavedItems.FavoriteRequests;
            _serializable.SerializeObjectToXml(SavedItems.FavoriteRequests, "favoriteRequests");
        }
        #endregion
    }
}
