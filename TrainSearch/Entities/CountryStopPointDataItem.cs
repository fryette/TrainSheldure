using System;

namespace TrainSearch.Entities
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
