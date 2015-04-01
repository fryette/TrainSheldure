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

       public CountryStopPointDataGroup()
       {
       }

       public string UniqueId { get; set; }
        public string Title { get; set; }
        public ObservableCollection<CountryStopPointDataItem> Items { get; set; }
    }
}
