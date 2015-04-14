using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Model.Entities
{
    public enum Picture
    {
        International = 0,
        InterRegionalBusiness = 1,
        InterRegionalEconom = 2,
        RegionalBusiness = 3,
        RegionalEconom = 4,
        City = 5,
        Foreign = 6
    }

    public enum Carriage
    {
        FirstClassSleeper = 0,
        CompartmentSleeper = 1,
        EconomyClassSleeper = 2,
        SeatingCoaches1 = 3,
        SeatingCoaches2 = 4,
        MultipleUnitCars1 = 5,
        MultipleUnitCars2 = 6,
        MultipleUnitCoach = 7
    }

    public enum AboutPicture
    {
        Market = 0,
        Settings = 1,
        Mail = 2,
        AboutApp = 3,
    }
}
