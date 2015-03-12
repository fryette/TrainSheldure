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

        public bool CheckDate(DateTimeOffset datum)
        {
            if ((datum.Date - DateTime.Now).Days < 0)
            {
                ShowMessageBox(DateUpTooLater);
                return true;
            }
            if (datum.Date <= DateTime.Now.AddDays(45)) return false;
            ShowMessageBox(DateTooBig);
            return true;
        }

        public bool CheckInput(string from, string to, DateTimeOffset datum)
        {
            if (CheckDate(datum) || String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to) ||
                (!SavedItems.AutoCompletion.Any(x => x.UniqueId.Contains(from.Split('(')[0])) ||
                 !SavedItems.AutoCompletion.Any(x => x.UniqueId.Contains(to.Split('(')[0]))))
            {
                ShowMessageBox(IncorrectInput);
                return false;
            }
            if (NetworkInterface.GetIsNetworkAvailable()) return true;
            ShowMessageBox(ConectionError);
            return false;
        }

        public bool CheckFavorite(string from, string to)
        {
            return SavedItems.FavoriteRequests != null && string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to) && SavedItems.FavoriteRequests.Any(x => x.From == from && x.To == to);
        }
    }
}
