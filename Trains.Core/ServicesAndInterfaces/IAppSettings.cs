﻿using System.Collections.Generic;
using Trains.Model.Entities;

namespace Trains.Core.ServicesAndInterfaces
{
   public interface IAppSettings
    {
        List<LastRequest> LastRequests { get; set; }

        List<LastRequest> FavoriteRequests { get; set; }

        IEnumerable<CountryStopPointDataItem> AutoCompletion { get; set; }

        LastRequest UpdatedLastRequest { get; set; }

        //public static ResourceLoader ResourceLoader;
        //Language Lang { get; set; }
    }
}
