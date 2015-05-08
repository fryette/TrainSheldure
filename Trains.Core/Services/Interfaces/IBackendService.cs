using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Core.Services.Interfaces
{
    public interface IBackendService
    {

        Task<ChygunkaSettings> GetAppSettings();

    }
}

