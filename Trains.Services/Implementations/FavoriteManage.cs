using System.Collections.Generic;
using System.Linq;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;

namespace Trains.Services.Implementations
{
    public class FavoriteManage : IFavoriteManageService
    {
        private const string FavoriteStr = "favoriteRequests";
        private readonly ISerializableService _serializable;

        public FavoriteManage(ISerializableService serializable)
        {
            _serializable = serializable;
        }

        public void DeleteFavorite(List<LastRequest> favoriteList)
        {
            foreach (var lastRequest in favoriteList.Where(x => x.IsCanBeDeleted))
                SavedItems.FavoriteRequests.Remove(lastRequest);
            favoriteList = SavedItems.FavoriteRequests;
            if (!favoriteList.Any())
            {
                _serializable.DeleteFile(FavoriteStr);
                ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("AllFavoritesDeleted"));
            }
            _serializable.SerializeObjectToXml(SavedItems.FavoriteRequests, FavoriteStr);
        }
    }
}
