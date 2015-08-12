using System;
using Trains.Model.Entities;

namespace Trains.Entities
{
    public class Train
    {
        public string City { get; set; }
        public string BeforeDepartureTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public string TrainNote { get; set; }
        public TrainClass Image { get; set; }
        public string Type { get; set; }
        public AdditionalInformation[] AdditionalInformation { get; set; }
        public string OnTheWay { get; set; }
        public string DepartureDate { get; set; }
        public string Link { get; set; }
        public string IsPlace { get; set; }
        public PlaceClasses PlaceClasses { get; set; }
        
        public bool InternetRegistration { get; set; }

    }
}

public class AdditionalInformation
{
    public string Name { get; set; }
    public string Price { get; set; }
    public string Place { get; set; }
}
