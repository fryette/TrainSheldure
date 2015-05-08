using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Trains.Model.Entities;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace Trains.WP.Converter
{
    public class EnumToImagePathConverter : IValueConverter
    {
        static readonly Dictionary<ShareSocial, Uri> SocialPicture = new Dictionary<ShareSocial, Uri>()
            {
                {ShareSocial.Vkontakte,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.vkontakte.png")},
                {ShareSocial.Facebook,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.social.facebook.heart.png")},
                {ShareSocial.Twitter,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.social.twitter.png")},
                {ShareSocial.GooglePlus,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.googleplus.png")},
                {ShareSocial.LinkedIn,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.social.linkedin.png")},
                {ShareSocial.Odnoklassniki,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.odnoklassniki.png")}
            };

        static readonly Dictionary<TrainClass, Uri> HelpPicture = new Dictionary<TrainClass, Uri>()
            {
                {TrainClass.International,new Uri(@"ms-appx:///Assets/HelpPageIcons/IL.png")},
                {TrainClass.City,new Uri(@"ms-appx:///Assets/HelpPageIcons/GE.png")},
                {TrainClass.InterRegionalBusiness,new Uri(@"ms-appx:///Assets/HelpPageIcons/IRLB_IRLE.png")},
                {TrainClass.RegionalBusiness,new Uri(@"ms-appx:///Assets/HelpPageIcons/RLB_RLE.png")}
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

        readonly Dictionary<Carriage, Uri> CarriagePictures;
        public EnumToImagePathConverter()
        {
            CarriagePictures = new Dictionary<Carriage, Uri>()
            {
                {Carriage.FirstClassSleeper,new Uri(@"ms-appx:///Assets/Carriage/FirstClassSleeper.png")},
                {Carriage.CompartmentSleeper,new Uri(@"ms-appx:///Assets/Carriage/CompartmentSleeper.png")},
                {Carriage.EconomyClassSleeper,new Uri(@"ms-appx:///Assets/Carriage/EconomyClassSleeper.png")},
                {Carriage.SeatingCoaches1,new Uri(@"ms-appx:///Assets/Carriage/SeatingCoaches1.png")},
                {Carriage.SeatingCoaches2,new Uri(@"ms-appx:///Assets/Carriage/SeatingCoaches2.png")},
                {Carriage.MultipleUnitCars1,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCars1.png")},
                {Carriage.MultipleUnitCars2,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCars2.png")},
                {Carriage.MultipleUnitCoach,new Uri(@"ms-appx:///Assets/Carriage/MultipleUnitCoach.png")}
            };
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var param = (string)parameter;
            if (param == "Help") return new BitmapImage(HelpPicture[(TrainClass)value]);
            if (param == "SocialPicture") return new BitmapImage(SocialPicture[(ShareSocial)value]);
            if (param == "Carriage") return new BitmapImage(CarriagePictures[(Carriage)value]);
            if (param == "TrainClass") return new SolidColorBrush(Images[(int)(TrainClass)value]);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
