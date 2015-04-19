using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Trains.Model.Entities;
using Trains.Resources;

namespace Trains.Core.ViewModels
{
    public abstract class BaseSearchViewModel : MvxViewModel
    {
        public async Task<bool> CheckInput(DateTimeOffset datum, string from, string to, List<CountryStopPointItem> autoCompletion)
        {
            if ((datum.Date - DateTime.Now).Days < 0)
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("DateUpTooLater"));
                return true;
            }
            if (datum.Date > DateTime.Now.AddDays(45))
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("DateTooBig"));
                return true;
            }

            if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to) ||
                !(autoCompletion.Any(x => x.UniqueId == from) &&
                  autoCompletion.Any(x => x.UniqueId == to)))
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("IncorrectInput"));
                return true;
            }

            if (NetworkInterface.GetIsNetworkAvailable()) return false;
            await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("ConectionError"));
            return true;
        }

        /// <summary>
        /// Update prompts during user input stopping point
        /// </summary>
        public List<string> UpdateAutoSuggestions(string str, List<CountryStopPointItem> autoCompletion)
        {
            if (string.IsNullOrEmpty(str)) return null;
            var autoSuggestions = autoCompletion.Where(x => x.UniqueId.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0).Select(x => x.UniqueId).ToList();
            if (autoSuggestions.Count == 1 && autoSuggestions[0] == str) return null;
            return autoSuggestions;
        }

        public List<LastRequest> UpdateLastRequests(List<LastRequest> lastRequests, string from, string to)
        {
            if (lastRequests == null) lastRequests = new List<LastRequest>(3);
            if (lastRequests.Any(x => x.From == from && x.To == to)) return lastRequests;
            if (lastRequests.Count == 3)
            {
                lastRequests[2] = lastRequests[1];
                lastRequests[1] = lastRequests[0];
                lastRequests[0] = new LastRequest { From = from, To = to };
            }

            else
                lastRequests.Add(new LastRequest { From = from, To = to });

            return lastRequests;
        }
    }
}
