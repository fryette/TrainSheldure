using System;
using System.Globalization;
using Windows.UI.Popups;
using Trains.Model.Entities;

namespace Trains.WP.Tools
{
    public static class ToolHelper
    {

        public static void ShowMessageBox(string message)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                (Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    var dialog = new MessageDialog(message);
                    await dialog.ShowAsync();
                });

        }
        public static string GetDate(DateTimeOffset datum, string selectedVariantOfSearch = null)
        {
            if (selectedVariantOfSearch == "На все дни") return "everyday";
            if (datum < DateTime.Now) datum = DateTime.Now;
            return datum.ToString("yy-MM-dd", CultureInfo.CurrentCulture);
        }
    }
}
