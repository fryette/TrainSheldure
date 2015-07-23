using System.Collections.Generic;
using Trains.Model.Entities;

namespace Trains.Core.Interfaces
{
	public interface IFavoriteManageService
	{
		void ManageFavorite(List<LastRequest> favoriteList);

		bool AddToFavorite(string from, string to);

		bool DeleteRoute(string from, string to);
	}
}
