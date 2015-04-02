using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Trains.Model.Entities
{
    public class CountryStopPointDataGroup
    {

        public string UniqueId { get; set; }
        public string Title { get; set; }

        public List<CountryStopPointDataItem> Items { get; set; }
    }
}
