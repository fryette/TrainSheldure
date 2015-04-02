using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Infrastructure.Interfaces;
using Trains.Services.Interfaces;
using Windows.Data.Json;
using Windows.Storage;

namespace Trains.WP.Implementations
{
    public class FileSystem : IFileSystem
    {
        public async Task<string> GetFileContents(string path)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
            return await FileIO.ReadTextAsync(file);
        }
    }
}
