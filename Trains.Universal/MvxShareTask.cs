using System;
using Windows.ApplicationModel.DataTransfer;
using Cirrious.MvvmCross.Plugins.Share;

namespace Trains.UAP
{
    public class MvxShareTask : IMvxShareTask
    {

        public void ShareLink(string title, string message, string link)
        {
            throw new NotImplementedException();
        }

        public void ShareShort(string message)
        {
            DataTransferManager dtManager = DataTransferManager.GetForCurrentView();
            dtManager.DataRequested += dtManager_DataRequested;
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        private async void dtManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            //Get the request object
            DataRequest request = e.Request;

            //Setup sharing properties
            request.Data.Properties.Title = "Share Text from WP 8.1";
            //request.Data.Properties.Description = "A demonstration that shows how to share text.";
            //Set shared data
            request.Data.SetText("Hello WP Blue!");
        }
    }
}
