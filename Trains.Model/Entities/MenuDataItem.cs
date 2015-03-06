using System;

namespace TrainSearch.Entities
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class MenuDataItem
    {
        public MenuDataItem(String uniqueId, String title, String imagePath, String description)
        {
            UniqueId = uniqueId;
            Title = title;
            ImagePath = imagePath;
            Description = description;

        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string ImagePath { get; private set; }
        public bool IsEconom { get; set; }
        public bool SpecialSearch { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Description { get; private set; }


        public override string ToString()
        {
            return Title;
        }
    }
}