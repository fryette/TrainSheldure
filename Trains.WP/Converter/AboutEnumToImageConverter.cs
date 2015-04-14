using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Model.Entities;
using Windows.UI.Xaml.Media.Imaging;

namespace Trains.WP.Converter
{
    public class AboutEnumToImageConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        static Dictionary<AboutPicture, Uri> openWith =
            new Dictionary<AboutPicture, Uri>()
            {
                {AboutPicture.AboutApp,new Uri(@"ms-appx:///Assets/appbar.information.png")},
                {AboutPicture.Mail,new Uri(@"ms-appx:///Assets/appbar.email.gmail.png")},
                {AboutPicture.Market,new Uri(@"ms-appx:///Assets/appbar.star.png")},
                {AboutPicture.Settings,new Uri(@"ms-appx:///Assets/appbar.settings.png")},
            };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            BitmapImage bmi = new BitmapImage(openWith[(AboutPicture)value]);
            return bmi;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
