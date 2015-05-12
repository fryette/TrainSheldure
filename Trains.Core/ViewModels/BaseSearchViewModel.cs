using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Resources;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public abstract class BaseSearchViewModel : MvxViewModel
    {
        public async Task<bool> CheckInput(DateTimeOffset datum, string from, string to, List<CountryStopPointItem> autoCompletion)
        {
            if ((datum.Date - DateTime.Now).Days < 0)
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["DateUpTooLater"]);
                return true;
            }
            if (datum.Date > DateTime.Now.AddDays(45))
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["DateTooBig"]);
                return true;
            }

            if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to) ||
                !(autoCompletion.Any(x => x.value == from.Trim()) &&
                  autoCompletion.Any(x => x.value == to.Trim())))
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["IncorrectInput"]);
                return true;
            }

            if (NetworkInterface.GetIsNetworkAvailable()) return false;
            await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["ConectionError"]);
            return true;
        }

        public List<LastRequest> UpdateLastRequests(List<LastRequest> lastRequests, string from, string to)
        {
            if (lastRequests == null) lastRequests = new List<LastRequest>(3);
            if (lastRequests.Any(x => x.Route.From == from && x.Route.To == to)) return lastRequests;
            if (lastRequests.Count == 3)
            {
                lastRequests[2] = lastRequests[1];
                lastRequests[1] = lastRequests[0];
                lastRequests[0] = new LastRequest { Route = new Route { From = from, To = to } };
            }

            else
                lastRequests.Add(new LastRequest { Route = new Route { From = from, To = to } });

            return lastRequests;
        }
    }
}
