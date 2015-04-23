using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Core.Interfaces
{
    public interface IFavoriteManageService
    {
		void ManageFavorite(List<LastRequest> favoriteList);

		Task<bool> AddToFavorite(string from, string to);

		Task<bool> DeleteRoute(string from, string to);
    }
}
