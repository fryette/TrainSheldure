using System.Collections.Generic;
using Trains.Core.Interfaces;
using Trains.Entities;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.Core
{
    public class AppSettings : IAppSettings
    {
        public List<LastRequest> LastRequests { get; set; }
        public List<LastRequest> FavoriteRequests { get; set; }
        public IEnumerable<CountryStopPointItem> AutoCompletion { get; set; }
        public LastRequest UpdatedLastRequest { get; set; }
        public IEnumerable<HelpInformationItem> HelpInformation { get; set; }
        public List<Train> LastRequestTrain { get; set; }


        public System.Resources.ResourceManager Resource { get; set; }
    }
}