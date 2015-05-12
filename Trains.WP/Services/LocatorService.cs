using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Core.Services.Interfaces;
using Trains.Model.Entities;
using Windows.Devices.Geolocation;

namespace Trains.WP.Services
{
    public class LocatorService : ILocatorService
    {
        Geolocator geo = null;
        public async Task<Coordinates> FindLocation()
        {
            if (geo == null)
            {
                geo = new Geolocator();
            }

            try
            {
                var position = await geo.GetGeopositionAsync();
                return new Coordinates { Lat = position.Coordinate.Latitude, Lon = position.Coordinate.Longitude };
            }

            // Failed - location capability not set in manifest, or user blocked location access at run-time.
            catch (System.UnauthorizedAccessException)
            {
                return null;
            }

            // Cancelled or timed-out.
            catch (TaskCanceledException)
            {
                return null;
            }
        }
    }
}
