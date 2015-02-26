using System;

namespace TrainShedule_HubVersion.Entities
{
  public class CountryStopPointDataItem
    {
        public string UniqueId { get; set; }
        public CountryStopPointDataItem(String uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}
