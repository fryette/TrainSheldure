using System;
using System.Globalization;
using Windows.UI.Popups;

namespace Trains.Services.Tools
{
    public static class ToolHelper
    {
        private const string Everyday = "everyday";
        private const string EveryDaySelectionMode = "На все дни";

        public static async void ShowMessageBox(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
        public static string GetDate(DateTimeOffset datum, string selectedVariantOfSearch = null)
        {
            if (selectedVariantOfSearch == EveryDaySelectionMode) return Everyday;
            if (datum < DateTime.Now) datum = DateTime.Now;
            return datum.ToString("yy-MM-dd",CultureInfo.CurrentCulture);
        }
    }
}
