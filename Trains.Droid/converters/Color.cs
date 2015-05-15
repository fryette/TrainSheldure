using Cirrious.CrossCore.UI;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Plugins.Color;
using System.Globalization;
using Trains.Model.Entities;


namespace Trains.Droid
{
	public class Color : MvxColorValueConverter
	{
		private static readonly MvxColor[] Images = {
			
			new MvxColor(255,230,0),
			new MvxColor(9,110,23),
			new MvxColor(0,185,25),
			new MvxColor(0,0,255),
			new MvxColor(0,0,142),
			new MvxColor(165,0,0),
			new MvxColor(255,255,255),
		};
		protected override MvxColor Convert(object value, object parameter, CultureInfo culture)
		{
			return Images[(int)(TrainClass)value];
		}

		public object ConvertBack (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new System.NotImplementedException ();
		}
	}
}