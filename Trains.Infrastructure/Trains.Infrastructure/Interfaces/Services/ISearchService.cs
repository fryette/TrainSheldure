using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Model;
using Trains.Model.Entities;

namespace Trains.Infrastructure.Interfaces.Services
{
	public interface ISearchService
	{
		Task<IEnumerable<TrainModel>> GetTrainSchedule(CountryStopPointItem @from, CountryStopPointItem to, DateTimeOffset datum, string selectedVariant);
	}
}
