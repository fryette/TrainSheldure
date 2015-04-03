using System.Collections.Generic;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.Services.Implementation
{
    public class AppSettings : IAppSettings
    {
        public List<LastRequest> LastRequests { get; set; }
        public List<LastRequest> FavoriteRequests { get; set; }
        public IEnumerable<CountryStopPointItem> AutoCompletion { get; set; }
        public LastRequest UpdatedLastRequest { get; set; }
    }
}
