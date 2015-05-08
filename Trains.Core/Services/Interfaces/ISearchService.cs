using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Entities;
using Trains.Model.Entities;

namespace Trains.Core.Services.Interfaces
{
    public interface ISearchService
    {
        Task<List<Train>> GetTrainSchedule(CountryStopPointItem from, CountryStopPointItem to, DateTimeOffset datum, string SelectedVariant);
    }
}
