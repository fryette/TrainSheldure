using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Core.Services.Interfaces
{
   public interface ILocatorService
    {
       Task<Coordinates> FindLocation();
    }
}
