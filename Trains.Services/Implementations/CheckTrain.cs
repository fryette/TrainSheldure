using System;
using System.Linq;
using System.Net.NetworkInformation;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;

namespace Trains.Services.Implementations
{
    public class CheckTrain : ICheckTrainService
    {
        public bool CheckInput(string from, string to, DateTimeOffset datum)
        {
            if ((datum.Date - DateTime.Now).Days < 0)
            {
                ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("DateUpTooLater"));
                return true;
            }
            if (datum.Date > DateTime.Now.AddDays(45))
            {
                ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("DateTooBig"));
                return true;
            }

            if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to) ||
                !(SavedItems.AutoCompletion.Any(x => x.UniqueId == from) &&
                  SavedItems.AutoCompletion.Any(x => x.UniqueId == to)))
            {
                ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("IncorrectInput"));
                return true;
            }

            if (NetworkInterface.GetIsNetworkAvailable()) return false;
            ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("ConectionError"));
            return true;
        }

        public bool CheckFavorite(string from, string to)
        {
            if (SavedItems.FavoriteRequests == null || !SavedItems.FavoriteRequests.Any()) return true;
            return !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to) && !SavedItems.FavoriteRequests.Any(x => x.From == from && x.To == to);
        }
    }
}
