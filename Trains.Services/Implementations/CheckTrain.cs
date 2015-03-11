using System;
using System.Linq;
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.Services.Implementations
{
   public class CheckTrain : ICheckTrainService
    {
        private const string DateUpTooLater = "Поиск может производится начиная от текущего времени";
        private const string DateTooBig = "Поиск может производится только за 45 дней от текущего момента или используйте режим \"На все дни\"";
        private const string IncorrectInput = "Один или оба пункта не существует, проверьте еще раз ввод";
        private const string ConectionError = "Проверьте подключение к интернету";


        public async void ShowMessageBox(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        public string GetDate(DateTimeOffset datum, string selectedVariantOfSearch)
        {
            if (selectedVariantOfSearch == "everyday") return "everyday";
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
    }
}
