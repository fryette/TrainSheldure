using System;
using System.Collections.ObjectModel;

namespace Trains.Model.Entities
{
    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class HelpInformationGroup
    {
        public HelpInformationGroup(String uniqueId, String title)
        {
            UniqueId = uniqueId;
            Title = title;
            Items = new ObservableCollection<HelpInformationItem>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public ObservableCollection<HelpInformationItem> Items { get; private set; }

        public override string ToString()
        {
            return Title;
        }
    }
}