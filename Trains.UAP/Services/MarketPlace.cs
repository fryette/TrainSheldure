using System;
using Windows.System;
using Trains.Core.Services.Interfaces;

namespace Trains.UAP.Services
{
    public class MarketPlace:IMarketPlaceService
    {
        public async void GoToMarket()
        {
            var uri = new Uri("ms-windows-store:reviewapp?appid=9a0879a6-0764-4e99-87d7-4c1c33f2d78e");
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
