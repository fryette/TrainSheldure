using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Trains.Model.Entities;

namespace Trains.WP.Converters
{
	public class EnumToImagePathConverter : IValueConverter
	{
		static readonly Dictionary<ShareSocial, string> SocialPicture = new Dictionary<ShareSocial, string>()
			{
				{ShareSocial.VKONTAKTE,"ms-appx:///Assets/AppBarIcons/appbar.vkontakte"},
				{ShareSocial.FACEBOOK,"ms-appx:///Assets/AppBarIcons/appbar.social.facebook.heart"},
				{ShareSocial.TWITTER,"ms-appx:///Assets/AppBarIcons/appbar.social.twitter"},
				{ShareSocial.GOOGLEPLUS,"ms-appx:///Assets/AppBarIcons/appbar.googleplus"},
				{ShareSocial.LINKEDIN,"ms-appx:///Assets/AppBarIcons/appbar.social.linkedin"},
				{ShareSocial.ODNOKLASSNIKI,"ms-appx:///Assets/AppBarIcons/appbar.odnoklassniki"}
			};

		static readonly Dictionary<TrainClass, Uri> HelpPicture = new Dictionary<TrainClass, Uri>()
			{
				{TrainClass.INTERNATIONAL,new Uri(@"ms-appx:///Assets/HelpPageIcons/IL.png")},
				{TrainClass.CITY,new Uri(@"ms-appx:///Assets/HelpPageIcons/GE.png")},
				{TrainClass.INTERREGIONALBUSINESS,new Uri(@"ms-appx:///Assets/HelpPageIcons/IRLB_IRLE.png")},
				{TrainClass.REGIONALBUSINESS,new Uri(@"ms-appx:///Assets/HelpPageIcons/RLB_RLE.png")}
			};


		private static readonly Color[] Images = {
			Colors.Black,
			Colors.Yellow,
			Colors.DarkGreen,
			Colors.Green,
			Colors.RoyalBlue,
			Colors.Blue,
			Colors.Red,
			Colors.WhiteSmoke
		};

		readonly Dictionary<Carriage, Uri> _carriagePictures;
		public EnumToImagePathConverter()
		{
			_carriagePictures = new Dictionary<Carriage, Uri>()
			{
				{Carriage.FIRSTCLASSSLEEPER,new Uri(@"ms-appx:///Assets/Carriage/FirstClassSleeper.png")},
				{Carriage.COMPARTMENTSLEEPER,new Uri(@"ms-appx:///Assets/Carriage/CompartmentSleeper.png")},
				{Carriage.ECONOMYCLASSSLEEPER,new Uri(@"ms-appx:///Assets/Carriage/EconomyClassSleeper.png")},
				{Carriage.SEATINGCOACHES1,new Uri(@"ms-appx:///Assets/Carriage/SeatingCoaches1.png")},
				{Carriage.SEATINGCOACHES2,new Uri(@"ms-appx:///Assets/Carriage/SeatingCoaches2.png")},
				{Carriage.MULTIPLEUNITCARS1,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCars1.png")},
				{Carriage.MULTIPLEUNITCARS2,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCars2.png")},
				{Carriage.MULTIPLEUNITCOACH,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCoach.png")}
			};
		}

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var param = (string)parameter;
			switch (param)
			{
				case "Help":
					return new BitmapImage(HelpPicture[(TrainClass)value]);
				case "SocialPicture":
					return new BitmapImage(new Uri(SocialPicture[(ShareSocial)value] +
												   (((App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush).Color).R == 0 ? "Black.png" : "White.png")));
				case "Carriage":
					return new BitmapImage(_carriagePictures[(Carriage)value]);
				case "TrainClass":
					return new SolidColorBrush(Images[(int)(TrainClass)value]);
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
