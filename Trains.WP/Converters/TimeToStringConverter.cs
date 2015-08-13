using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace Trains.WP.Converters
{
	public class TimeToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return ((DateTime)value).ToString("t", CultureInfo.InvariantCulture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
