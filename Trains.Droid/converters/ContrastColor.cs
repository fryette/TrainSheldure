using Cirrious.CrossCore.UI;
using Cirrious.CrossCore.Converters;
namespace Trains.Droid
{
	public class ContrastColor : IMvxValueConverter
{
		public object Convert (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var intVal = value as int?;

			if (intVal == null)
				return (new MvxColor(0, 0, 0, 150));

			switch (intVal.Value)
			{
			case -1:
				return (new MvxColor(255, 0, 0, 150));
			case 0:
				return (new MvxColor(0, 0, 0, 150));
			case 1:
				return (new MvxColor(0, 255, 0, 150));
			}
			return (new MvxColor(0, 0, 0, 150));
		}

		public object ConvertBack (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new System.NotImplementedException ();
		}
}
}