namespace TrainShedule_HubVersion.Infrastructure
{
    public class Train
    {
        public string City { get; set; }
        public string BeforeDepartureTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string TrainNote { get; set; }
        public string ImagePath { get; set; }
        public string Type { get; set; }
        public AdditionalInformation[] AdditionalInformation { get; set; }
        public string OnTheWay { get; set; }
        public string DepartureDate { get; set; }
        public string Link { get; set; }
    }
}

public class AdditionalInformation
{
    public string Name { get; set; }
    public string Price { get; set; }
    public string Place { get; set; }
}
