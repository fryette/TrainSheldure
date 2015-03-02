using System;

namespace TrainShedule_HubVersion.Entities
{
    public class CountryStopPointDataItem
    {
        public string UniqueId { get; set; }
        public string Country { get; set; }
        public CountryStopPointDataItem(String uniqueId, String country)
        {
            UniqueId = uniqueId;
            Country = country;
        }
    }
}
