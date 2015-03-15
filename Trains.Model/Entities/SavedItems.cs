using System.Collections.Generic;
using Trains.Entities;

namespace Trains.Model.Entities
{
    public static class SavedItems
    {
        public static List<LastRequest> LastRequests { get; set; }
    
        public static List<LastRequest> FavoriteRequests { get; set; }

        public static IEnumerable<CountryStopPointDataItem> AutoCompletion { get; set; }
    }
}
