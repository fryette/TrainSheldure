using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Trains.Universal.Converter
{
    public class ForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = (App.Current.Resources["SystemControlForegroundAccentBrush"] as SolidColorBrush).Color;
            return new SolidColorBrush((color.R + color.G + color.B) / 3 > 127 ? Colors.Black:Colors.White) ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
