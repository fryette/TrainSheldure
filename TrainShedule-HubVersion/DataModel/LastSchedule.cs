using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainShedule_HubVersion.Data;

namespace TrainShedule_HubVersion.DataModel
{
   static class LastSchedule
    {
        public static IEnumerable<Train> LastShedule{get;set;}

        static LastSchedule()
        {
        }

        static async void SetShedule()
        {
            LastShedule=await Serialize.ReadObjectFromXmlFileAsync1("LastTrainList");
        }
    }
}
