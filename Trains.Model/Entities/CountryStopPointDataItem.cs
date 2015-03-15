using System;

namespace Trains.Model.Entities
{
    public class CountryStopPointDataItem
    {
        public string UniqueId { get; set; }
        public string Country { get; set; }
        public string Exp { get; set; }

        public CountryStopPointDataItem(String uniqueId, String country, String exp)
        {
            UniqueId = uniqueId;
            Country = country;
            Exp = exp;
        }
    }
}
