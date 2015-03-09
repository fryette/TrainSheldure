using System;
using Windows.UI.Xaml.Data;

namespace Trains.App.Converter
{
    public class BooleanToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            return (value is bool && (bool)value) ? new Uri("/Assets/appbar.delete.png", UriKind.Relative) : new Uri("/Assets/appbar.delete.png", UriKind.Relative);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new Exception();
        }
    }
}
