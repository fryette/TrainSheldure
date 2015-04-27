using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Core.Interfaces;

namespace Trains.WP.Services
{
    public class Analytics : IAnalytics
    {
        public void SentView(string view)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendView(view);
        }

        public void SentEvent(string mainCategory, string subCategory1 = "", string subCategory2 = "", long value = 0)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendEvent(mainCategory, subCategory1, subCategory2, value);
        }

        public void SentException(string description, bool isFatal = false)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendException(description, isFatal);
        }
    }
}
