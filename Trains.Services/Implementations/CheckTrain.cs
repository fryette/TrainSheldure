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
        private readonly IAppSettings _appSettings;

        public CheckTrain(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public bool CheckInput(string from, string to, DateTimeOffset datum)
        {
            if ((datum.Date - DateTime.Now).Days < 0)
            {
                //ToolHelper.ShowMessageBox(_appSettings.ResourceLoader.GetString("DateUpTooLater"));
                return true;
            }
            if (datum.Date > DateTime.Now.AddDays(45))
            {
                //ToolHelper.ShowMessageBox(_appSettings.ResourceLoader.GetString("DateTooBig"));
                return true;
            }

            if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to) ||
                !(_appSettings.AutoCompletion.Any(x => x.UniqueId == from) &&
                  _appSettings.AutoCompletion.Any(x => x.UniqueId == to)))
            {
                //ToolHelper.ShowMessageBox(_appSettings.ResourceLoader.GetString("IncorrectInput"));
                return true;
            }

            if (NetworkInterface.GetIsNetworkAvailable()) return false;
            //ToolHelper.ShowMessageBox(_appSettings.ResourceLoader.GetString("ConectionError"));
            return true;
        }

        public bool CheckFavorite(string from, string to)
        {
            if (_appSettings.FavoriteRequests == null || !_appSettings.FavoriteRequests.Any()) return true;
            return !string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to) && !_appSettings.FavoriteRequests.Any(x => x.From == from && x.To == to);
        }
    }
}
