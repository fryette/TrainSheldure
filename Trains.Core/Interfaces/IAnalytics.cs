using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Core.Interfaces
{
    public interface IAnalytics
    {
        void SentView(string view);
        void SentEvent(string mainCategory, string subCategory1 = "", string subCategory2 = "", long value = 0);
        void SentException(string description, bool isFatal = false);
    }
}
