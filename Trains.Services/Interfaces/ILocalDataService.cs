using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Services.Interfaces
{
    public interface ILocalDataService
    {
        Task<List<CountryStopPointGroup>> GetStopPoints();
        Task<List<HelpInformationGroup>> GetHelpInformations();
    }
}
