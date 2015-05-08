﻿using System.Collections.Generic;
using System.Linq;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Model.Entities;

namespace Trains.Core
{
    public class FavoriteManage : IFavoriteManageService
    {
        private readonly ISerializableService _serializable;
        private readonly IAppSettings _appSettings;

        public FavoriteManage(ISerializableService serializable, IAppSettings appSettings)
        {
            _serializable = serializable;
            _appSettings = appSettings;
        }

        public void ManageFavorite(List<LastRequest> favoriteList)
        {
            foreach (var lastRequest in favoriteList.Where(x => x.IsCanBeDeleted))
                _appSettings.FavoriteRequests.Remove(lastRequest);
            favoriteList = _appSettings.FavoriteRequests;
            if (!favoriteList.Any())
            {
                _serializable.Delete(Constants.FavoriteRequests);
                //ToolHelper.ShowMessageBox(_appSettings.ResourceLoader.GetString("AllFavoritesDeleted"));
            }
            _serializable.Serialize(_appSettings.FavoriteRequests, Constants.FavoriteRequests);
        }

        public bool AddToFavorite(string from, string to)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
            {
                //ToolHelper.ShowMessageBox(_appSettings.ResourceLoader.GetString("PointsIsInCorrect"));
                return false;
            }

            if (_appSettings.FavoriteRequests == null) _appSettings.FavoriteRequests = new List<LastRequest>();
            if (_appSettings.FavoriteRequests.Any(x => x.From == from && x.To == to))
            {
                //ToolHelper.ShowMessageBox(_appSettings.ResourceLoader.GetString("ThisRouteIsPresent"));
                return false;
            }

            _appSettings.FavoriteRequests.Add(new LastRequest { From = from, To = to });
            _serializable.Serialize(_appSettings.FavoriteRequests, Constants.FavoriteRequests);
            //ToolHelper.ShowMessageBox(_appSettings.ResourceLoader.GetString("RouteIsAddedToFavorite"));
            return true;
        }

        public bool DeleteRoute(string from, string to)
        {
            var objectToDelete = _appSettings.FavoriteRequests.FirstOrDefault(x => x.From == from && x.To == to);
            if (objectToDelete == null) return false;
            _appSettings.FavoriteRequests.Remove(objectToDelete);
             _serializable.Serialize(_appSettings.FavoriteRequests, Constants.FavoriteRequests);
            //ToolHelper.ShowMessageBox(_appSettings.ResourceLoader.GetString("RouteIsDeletedInFavorite"));
            return true;
            //ToolHelper.ShowMessageBox(_appSettings.ResourceLoader.GetString("RouteIsIncorect"));
        }
    }
}
