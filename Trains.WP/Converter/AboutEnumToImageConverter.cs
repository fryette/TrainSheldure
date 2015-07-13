using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Trains.Model.Entities;

namespace Trains.WP.Converter
{
	public class AboutEnumToImageConverter : IValueConverter
	{
		static readonly Dictionary<AboutPicture, Uri> Pictures =
			new Dictionary<AboutPicture, Uri>()
			{
				{AboutPicture.AboutApp,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.information.png")},
				{AboutPicture.Mail,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.email.gmail.png")},
				{AboutPicture.Market,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.star.png")},
				{AboutPicture.Settings,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.settings.png")},
				{AboutPicture.Share,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.share.png")}
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
