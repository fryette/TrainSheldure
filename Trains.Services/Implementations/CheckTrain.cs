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
        public void ShowMessageBox(string message)
        {
            ToolHelper.ShowMessageBox(message);
        }

        public string CheckDate(DateTimeOffset datum)
        {
            if ((datum.Date - DateTime.Now).Days < 0)
                return SavedItems.ResourceLoader.GetString("DateUpTooLater");
            return datum.Date <= DateTime.Now.AddDays(45) ? null : SavedItems.ResourceLoader.GetString("DateTooBig");
        }

        public string CheckInput(string from, string to, DateTimeOffset datum)
        {
            var dayError = CheckDate(datum);
            if (dayError != null) return dayError;
            if (CheckDate(datum) != null || String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to) ||
                !(SavedItems.AutoCompletion.Any(x => x.UniqueId == from) && SavedItems.AutoCompletion.Any(x => x.UniqueId == to)))
                return SavedItems.ResourceLoader.GetString("IncorrectInput");
            return NetworkInterface.GetIsNetworkAvailable() ? null : SavedItems.ResourceLoader.GetString("ConectionError");
        }

        public bool CheckFavorite(string from, string to)
        {
            if (SavedItems.FavoriteRequests == null || !SavedItems.FavoriteRequests.Any()) return true;
            return !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to) && !SavedItems.FavoriteRequests.Any(x => x.From == from && x.To == to);
        }
    }
}
