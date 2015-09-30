using System;

namespace Trains.Models
{
    public class LastRequest
    {
        public Route Route { get; set; }
        public bool IsCanBeDeleted { get; set; }
        public DateTimeOffset Date { get; set; }
        public string SelectionMode { get; set; }
    }
}