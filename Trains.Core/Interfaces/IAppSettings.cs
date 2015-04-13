using System.Collections.Generic;
using System.Resources;
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

        IEnumerable<HelpInformationItem> HelpInformation { get; set; }

        List<Train> LastRequestTrain { get; set; }

        List<CarriageModel> CarriageModel { get; set; }

    }
}
