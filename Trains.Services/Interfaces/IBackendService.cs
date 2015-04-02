using System;
using Trains.Model;
using System.Threading.Tasks;

namespace Trains.Services
{
    public interface IBackendService
    {

        Task<ChygunkaSettings> GetAppSettings();

    }
}

