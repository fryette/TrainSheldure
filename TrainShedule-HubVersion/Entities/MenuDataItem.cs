using System;

namespace TrainShedule_HubVersion.Infrastructure
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class MenuDataItem
    {
        public MenuDataItem(String uniqueId, String title, String imagePath)
        {
            UniqueId = uniqueId;
            Title = title;
            ImagePath = imagePath;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string ImagePath { get; private set; }
        public bool IsEconom { get; set; }
        public bool SpecialSearch { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}