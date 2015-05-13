using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Trains.Core.Interfaces;

namespace Trains.Droid.Services
{
    public class Analytics : IAnalytics
    {
        public void SentView(string view)
        {
            
        }

        public void SentEvent(string mainCategory, string subCategory1 = "", string subCategory2 = "", long value = 0)
        {
        }

        public void SentException(string description, bool isFatal = false)
        {
        }
    }
}