using System;
using Windows.UI.Popups;

namespace Trains.Services.Tools
{
    public static class ToolHelper
    {
        public static async void ShowMessageBox(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
}
