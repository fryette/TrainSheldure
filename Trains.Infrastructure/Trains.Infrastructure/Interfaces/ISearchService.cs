using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Models;

namespace Trains.Infrastructure.Interfaces
{
	public interface ISearchService
	{
		Task<List<Train>> GetTrainSchedule(CountryStopPointItem from, CountryStopPointItem to, string datum);
	}
}
