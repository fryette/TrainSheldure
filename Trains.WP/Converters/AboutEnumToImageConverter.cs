using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Trains.Model.Entities;

namespace Trains.WP.Converters
{
	public class AboutEnumToImageConverter : IValueConverter
	{
		static readonly Dictionary<AboutPicture, Uri> Pictures =
			new Dictionary<AboutPicture, Uri>()
			{
				{AboutPicture.ABOUT_APP,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.information.png")},
				{AboutPicture.MAIL,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.email.gmail.png")},
				{AboutPicture.MARKET,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.star.png")},
				{AboutPicture.SETTINGS,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.settings.png")},
				{AboutPicture.SHARE,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.share.png")}
			};

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			BitmapImage bmi = new BitmapImage(Pictures[(AboutPicture)value]);
			return bmi;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
