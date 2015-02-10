using System.Collections.Generic;

namespace TrainShedule_HubVersion.DataModel
{
    public class Train
    {
        //public string To { get; set; }
        //public string From { get; set; }
        public string City { get; set; }
        public string BeforeDepartureTime { get; set; }
        //public string Price { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string TrainNote { get; set; }
        //public string Place { get; set; }
        public string ImagePath { get; set; }
        public string Type { get; set; }
    }
}
