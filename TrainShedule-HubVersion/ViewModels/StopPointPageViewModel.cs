using System.Collections.Generic;
using Caliburn.Micro;
using TrainSearch.Entities;

namespace TrainShedule_HubVersion.ViewModels
{
    public class StopPointPageViewModel : Screen
    {
        public IEnumerable<TrainStop> Parameter { get; set; }
        protected override void OnActivate() { }
    }
}
