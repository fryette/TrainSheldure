using System.Collections.Generic;
using Trains.Model.Entities;

namespace Trains.Services.Interfaces
{
    public interface IFavoriteManageService
    {
       void DeleteFavorite(List<LastRequest> favoriteList);
    }
}
