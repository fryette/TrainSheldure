using System.Collections.Generic;
//using Windows.ApplicationModel.Resources;

namespace Trains.Model.Entities
{
    public static class SavedItems
    {
        public static List<LastRequest> LastRequests { get; set; }
    
        public static List<LastRequest> FavoriteRequests { get; set; }

        public static IEnumerable<CountryStopPointDataItem> AutoCompletion { get; set; }

        public static LastRequest UpdatedLastRequest { get; set; }
        //public static ResourceLoader ResourceLoader;
        public static Language Lang { get; set; }
    }
}
