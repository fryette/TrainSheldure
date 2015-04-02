using System.Collections.Generic;
using Trains.Model.Entities;

namespace Trains.Core
{
  public class AppSettings:IAppSettings
    {
        public List<LastRequest> LastRequests { get; set; }
        public List<LastRequest> FavoriteRequests { get; set; }
        public IEnumerable<CountryStopPointDataItem> AutoCompletion { get; set; }
        public LastRequest UpdatedLastRequest { get; set; }
    }
}
