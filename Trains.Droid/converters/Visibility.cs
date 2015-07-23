
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore.Converters;
using System.Globalization;
using Cirrious.CrossCore.UI;

namespace Trains.Droid
{
	[Activity (Label = "Visibility")]			
	public class Visibility : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(parameter==null) 
				return ((bool)value)?ViewStates.Visible : ViewStates.Gone;
			if((string)parameter=="1")
				return ((bool)value)?ViewStates.Gone:ViewStates.Visible;
			if((string)parameter=="2")
				return ((string)value).Contains(":") ? ViewStates.Gone : ViewStates.Visible;
			if((string)parameter=="3")
				return ((string)value).Contains(":") ? ViewStates.Visible : ViewStates.Gone;
			return ViewStates.Visible;
			}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((int)value) - 1;
		}	
	}
}

