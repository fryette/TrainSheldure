using System.Collections.Generic;

namespace Trains.Model.Entities
{
    public static class SavedItems
    {
        public static List<LastRequest> LastRequests { get; set; }
    
        public static List<LastRequest> FavoriteRequests { get; set; }
    }
}
