using System.Collections.Generic;
using Trains.Entities;
using Trains.Model.Entities;

namespace Trains.Services.Interfaces
{
   public interface IAppSettings
    {
        List<LastRequest> LastRequests { get; set; }

        List<LastRequest> FavoriteRequests { get; set; }

        IEnumerable<CountryStopPointItem> AutoCompletion { get; set; }

        LastRequest UpdatedLastRequest { get; set; }

        IEnumerable<HelpInformationItem> HelpInformation { get; set; }

        List<Train> LastRequestTrain { get; set; }
        //public static ResourceLoader ResourceLoader;
        //Language Lang { get; set; }
    }
}
