using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Infrastructure.Interfaces
{
    public interface ILocalDataService
    {
        Task<List<CountryStopPointDataGroup>> GetData();
    }
}
