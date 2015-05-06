using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Trains.Model.Entities;

namespace Trains.WP.Converter
{
    class EnumToImagePathConverter : IValueConverter
    {
        private static readonly Color[] Images = {
            Colors.Yellow,
            Colors.DarkGreen,
            Colors.Green,
            Colors.DarkBlue,
            Colors.Blue,
            Colors.Red,
            Colors.WhiteSmoke

        };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new SolidColorBrush(Images[(int)(TrainClass)value]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
