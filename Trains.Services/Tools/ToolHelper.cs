using System;
using System.Globalization;
using Trains.Model.Entities;

namespace Trains.Services.Tools
{
    public static class ToolHelper
    {
        public static string GetDate(DateTimeOffset datum, string selectedVariantOfSearch = null)
        {
            if (selectedVariantOfSearch == "На все дни") return "everyday";
            if (datum < DateTime.Now) datum = DateTime.Now;
            return datum.ToString("yy-MM-dd", CultureInfo.CurrentCulture);
        }
    }
}
