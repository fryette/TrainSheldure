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
        private const string DateUpTooLater = "Поиск может производится начиная от текущего времени";
        private const string DateTooBig = "Поиск может производится только за 45 дней от текущего момента или используйте режим \"На все дни\"";
        private const string IncorrectInput = "Один или оба пункта не существует, проверьте еще раз ввод";
        private const string ConectionError = "Проверьте подключение к интернету";
        private const string Everyday = "everyday";

        public void ShowMessageBox(string message)
        {
            ToolHelper.ShowMessageBox(message);
        }

        public string GetDate(DateTimeOffset datum, string selectedVariantOfSearch)
        {
            if (selectedVariantOfSearch == Everyday) return Everyday;
            return datum.Date.Year + "-" + datum.Date.Month + "-" + datum.Date.Day;
        }

        public string CheckDate(DateTimeOffset datum)
        {
            if ((datum.Date - DateTime.Now).Days < 0)
            {
                return DateUpTooLater;
            }
            return datum.Date <= DateTime.Now.AddDays(45) ? null : DateTooBig;
        }

        public string CheckInput(string from, string to, DateTimeOffset datum)
        {
            var dayError = CheckDate(datum);
            if (dayError != null) return dayError;
            if (CheckDate(datum)!=null || String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to) ||
                !(SavedItems.AutoCompletion.Any(x => x.UniqueId == from) &&
                 SavedItems.AutoCompletion.Any(x => x.UniqueId == to)))
            {
                return IncorrectInput;
            }
            return NetworkInterface.GetIsNetworkAvailable() ? null : ConectionError;
        }

        public bool CheckFavorite(string from, string to)
        {
            if (SavedItems.FavoriteRequests == null || !SavedItems.FavoriteRequests.Any()) return true;
            return !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to) && !SavedItems.FavoriteRequests.Any(x => x.From == from && x.To == to);
        }
    }
}
