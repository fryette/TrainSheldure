using System.Collections.Generic;
using System.Linq;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.WP.Implementations
{
    public class FavoriteManage : IFavoriteManageService
    {
        private readonly ISerializableService _serializable;

        public FavoriteManage(ISerializableService serializable)
        {
            _serializable = serializable;
        }

        public void ManageFavorite(List<LastRequest> favoriteList)
        {
            foreach (var lastRequest in favoriteList.Where(x => x.IsCanBeDeleted))
                SavedItems.FavoriteRequests.Remove(lastRequest);
            favoriteList = SavedItems.FavoriteRequests;
            if (!favoriteList.Any())
            {
                _serializable.DeleteFile(FileName.FavoriteRequests);
                //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("AllFavoritesDeleted"));
            }
            _serializable.SerializeObjectToXml(SavedItems.FavoriteRequests, FileName.FavoriteRequests);
        }

        public bool AddToFavorite(string from, string to)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("PointsIsInCorrect"));
                return false;
            }

            if (SavedItems.FavoriteRequests == null) SavedItems.FavoriteRequests = new List<LastRequest>();
            if (SavedItems.FavoriteRequests.Any(x => x.From == from && x.To == to))
            {
                //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("ThisRouteIsPresent"));
                return false;
            }

            SavedItems.FavoriteRequests.Add(new LastRequest { From = from, To = to });
            _serializable.SerializeObjectToXml(SavedItems.FavoriteRequests, FileName.FavoriteRequests);
            //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("RouteIsAddedToFavorite"));
            return true;
        }

        public bool DeleteRoute(string from, string to)
        {
            var objectToDelete = SavedItems.FavoriteRequests.FirstOrDefault(x => x.From == from && x.To == to);
            if (objectToDelete != null)
            {
                SavedItems.FavoriteRequests.Remove(objectToDelete);
                _serializable.SerializeObjectToXml(SavedItems.FavoriteRequests, FileName.FavoriteRequests);
                //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("RouteIsDeletedInFavorite"));
                return true;
            }
            //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("RouteIsIncorect"));
            return false;
        }
    }
}
