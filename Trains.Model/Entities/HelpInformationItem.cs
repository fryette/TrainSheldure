using System;

namespace Trains.Model.Entities
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class HelpInformationItem
    {
        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string ImagePath { get; private set; }
        public string Description { get; private set; }
    }
}