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
				{ShareSocial.GOOGLE_PLUS,"ms-appx:///Assets/AppBarIcons/appbar.googleplus"},
				{ShareSocial.LINKED_IN,"ms-appx:///Assets/AppBarIcons/appbar.social.linkedin"},
				{ShareSocial.ODNOKLASSNIKI,"ms-appx:///Assets/AppBarIcons/appbar.odnoklassniki"}
			};

		static readonly Dictionary<TrainClass, Uri> HelpPicture = new Dictionary<TrainClass, Uri>()
			{
				{TrainClass.INTERNATIONAL,new Uri(@"ms-appx:///Assets/HelpPageIcons/IL.png")},
				{TrainClass.CITY,new Uri(@"ms-appx:///Assets/HelpPageIcons/GE.png")},
				{TrainClass.INTER_REGIONAL_BUSINESS,new Uri(@"ms-appx:///Assets/HelpPageIcons/IRLB_IRLE.png")},
				{TrainClass.REGIONAL_BUSINESS,new Uri(@"ms-appx:///Assets/HelpPageIcons/RLB_RLE.png")}
			};


		private static readonly Color[] Images = {
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
				{Carriage.FIRST_CLASS_SLEEPER,new Uri(@"ms-appx:///Assets/Carriage/FirstClassSleeper.png")},
				{Carriage.COMPARTMENT_SLEEPER,new Uri(@"ms-appx:///Assets/Carriage/CompartmentSleeper.png")},
				{Carriage.ECONOMY_CLASS_SLEEPER,new Uri(@"ms-appx:///Assets/Carriage/EconomyClassSleeper.png")},
				{Carriage.SEATING_COACHES1,new Uri(@"ms-appx:///Assets/Carriage/SeatingCoaches1.png")},
				{Carriage.SEATING_COACHES2,new Uri(@"ms-appx:///Assets/Carriage/SeatingCoaches2.png")},
				{Carriage.MULTIPLE_UNIT_CARS1,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCars1.png")},
				{Carriage.MULTIPLE_UNIT_CARS2,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCars2.png")},
				{Carriage.MULTIPLE_UNIT_COACH,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCoach.png")}
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
