using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Model.Entities
{
    public class PlaceClasses
    {
        //общий
        public bool General { get; set; }
        //сидячий
        public bool Sedentary { get; set; }
        //плацкартный
        public bool SecondClass { get; set; }
        //купе
        public bool Coupe { get; set; }
        //СВ
        public bool Luxury { get; set; }
    }
}
