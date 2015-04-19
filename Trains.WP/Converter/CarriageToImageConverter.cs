using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Trains.Model.Entities;

namespace Trains.WP.Converter
{
    public class CarriageToImageConverter : IValueConverter
    {
       static readonly Dictionary<Carriage, Uri> Pictures =
             new Dictionary<Carriage, Uri>()
            {
                {Carriage.FirstClassSleeper,new Uri(@"ms-appx:///Assets/Carriage/FirstClassSleeper.png")},
                {Carriage.CompartmentSleeper,new Uri(@"ms-appx:///Assets/Carriage/CompartmentSleeper.png")},
                {Carriage.EconomyClassSleeper,new Uri(@"ms-appx:///Assets/Carriage/EconomyClassSleeper.png")},
                {Carriage.SeatingCoaches1,new Uri(@"ms-appx:///Assets/Carriage/SeatingCoaches1.png")},
                {Carriage.SeatingCoaches2,new Uri(@"ms-appx:///Assets/Carriage/SeatingCoaches2.png")},
                {Carriage.MultipleUnitCars1,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCars1.png")},
                {Carriage.MultipleUnitCars2,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCars2.png")},
                {Carriage.MultipleUnitCoach,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCoach.png")}
            };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            BitmapImage bmi = new BitmapImage(Pictures[(Carriage)value]);
            return bmi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
