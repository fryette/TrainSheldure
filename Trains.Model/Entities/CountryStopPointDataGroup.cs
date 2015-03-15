using System;
using System.Collections.ObjectModel;

namespace Trains.Model.Entities
{
   public class CountryStopPointDataGroup
    {
        public CountryStopPointDataGroup(String uniqueId, String title)
        {
            UniqueId = uniqueId;
            Title = title;
            Items = new ObservableCollection<CountryStopPointDataItem>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public ObservableCollection<CountryStopPointDataItem> Items { get; private set; }
    }
}
