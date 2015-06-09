using GoogleAnalytics;
using Trains.Core.Interfaces;

namespace Trains.UAP.Services
{
    public class Analytics : IAnalytics
    {
        public void SentView(string view)
        {
            EasyTracker.GetTracker().SendView(view);
        }

        public void SentEvent(string mainCategory, string subCategory1 = "", string subCategory2 = "", long value = 0)
        {
            EasyTracker.GetTracker().SendEvent(mainCategory, subCategory1, subCategory2, value);
        }

        public void SentException(string description, bool isFatal = false)
        {
            EasyTracker.GetTracker().SendException(description, isFatal);
        }
    }
}
