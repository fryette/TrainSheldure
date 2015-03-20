using System;

namespace Trains.Model.Entities
{
    public class LastRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public bool IsCanBeDeleted { get; set; }
        public DateTimeOffset Date { get; set; }
        public string SelectionMode { get; set; }
    }
}