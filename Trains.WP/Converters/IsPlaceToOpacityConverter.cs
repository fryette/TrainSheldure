using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace Trains.WP.Converters
{
	public class IsPlaceToOpacityConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null) return 1;
			return ((string)value).Contains(':') ? 1 : 0.5;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
