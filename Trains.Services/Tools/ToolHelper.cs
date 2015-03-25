using System;
using System.Globalization;
using Windows.UI.Popups;
using Trains.Model.Entities;

namespace Trains.Services.Tools
{
    public static class ToolHelper
    {

        public static async void ShowMessageBox(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
        public static string GetDate(DateTimeOffset datum, string selectedVariantOfSearch = null)
        {
            if (selectedVariantOfSearch == SavedItems.ResourceLoader.GetString("AllDays")) return SavedItems.ResourceLoader.GetString("Everyday");
            if (datum < DateTime.Now) datum = DateTime.Now;
            return datum.ToString("yy-MM-dd", CultureInfo.CurrentCulture);
        }
    }
}
