using System;

namespace TrainScheduleBelarus.Infrastructure
{
    public class Train
    {
        public string To { get; set; }
        public string From { get; set; }
        public string City { get; set; }
        public string TotalTime { get; set; }
        public string Price { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string TrainNote { get; set; }
        public string Place { get; set; }
        public Train(String[] train)
        {
            this.City = train[0];
            this.TotalTime = train[4];
            this.StartTime = train[2];
            this.EndTime = train[3];
            this.TrainNote = train[5];
            this.Place = train[6];
            this.Price = train[7];
            this.Description = train[1];
            this.To = train[8];
            this.From = train[9];    
        }
    }
}