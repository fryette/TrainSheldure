﻿using System.Collections.Generic;
using Trains.Core.Interfaces;
using Trains.Entities;
using Trains.Model.Entities;

namespace Trains.Core
{
    public class AppSettings : IAppSettings
    {
        public List<LastRequest> LastRequests { get; set; }
        public List<LastRequest> FavoriteRequests { get; set; }
        public List<CountryStopPointItem> AutoCompletion { get; set; }
        public LastRequest UpdatedLastRequest { get; set; }
        public List<HelpInformationItem> HelpInformation { get; set; }
        public List<Train> LastRequestTrain { get; set; }

        public List<CarriageModel> CarriageModel { get; set; }
        public List<About> About { get; set; }
    }
}
