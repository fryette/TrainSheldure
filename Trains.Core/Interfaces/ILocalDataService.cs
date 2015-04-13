using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Core.Interfaces
{
    public interface ILocalDataService
    {
        Task<T> GetData<T>(string filename) where T : class;
    }
}
