using System;
using Cirrious.CrossCore.Converters;
using Android.App;

namespace Trains.Droid
{
	[Activity (Label = "ReverseBoolean")]				
	public class ReverseBoolean : IMvxValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return !(bool)value;
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
	}
}

