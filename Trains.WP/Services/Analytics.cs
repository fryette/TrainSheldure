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
    }
}
