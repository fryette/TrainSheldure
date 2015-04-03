using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Trains.Model.Entities
{
    public class CountryStopPointGroup
    {

        public string UniqueId { get; set; }
        public string Title { get; set; }

        public List<CountryStopPointItem> Items { get; set; }
    }
}
