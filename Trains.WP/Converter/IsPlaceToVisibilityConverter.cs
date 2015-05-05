using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Trains.WP.Converter
{
    public class IsPlaceToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter != null) return ((string)value).Contains(":") ? Visibility.Collapsed : Visibility.Visible;
            if (value != null)
                return ((string)value).Contains(":") ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
