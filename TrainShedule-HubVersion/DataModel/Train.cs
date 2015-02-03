using System;
using System.Collections.Generic;

namespace TrainShedule_HubVersion.Data
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
        public string Type { get; set; }
        public string Status { get; set; }
        public Train()
        {

        }
        public Train(IList<string> train)
        {
            City = train[2];
            Description = train[3];
            StartTime = train[0];
            EndTime = train[1];
            Type = train[4];
            Status = train[5];
            BeforeDepartureTime = train[6];
            //TrainNote = train[5];
            //Place = train[6];
            //Price = train[7];
            //To = train[8];
            //From = train[9];    
        }
    }
}
