﻿using System.Collections.Generic;
using Trains.Entities;
using Trains.Model.Entities;

namespace Trains.Core.Interfaces
{
   public interface IAppSettings
    {
        List<LastRequest> LastRequests { get; set; }

        List<LastRequest> FavoriteRequests { get; set; }

        List<CountryStopPointItem> AutoCompletion { get; set; }

        LastRequest UpdatedLastRequest { get; set; }

        List<HelpInformationItem> HelpInformation { get; set; }

        List<Train> LastRequestTrain { get; set; }

        List<CarriageModel> CarriageModel { get; set; }

        List<About> About { get; set; }

    }
}
