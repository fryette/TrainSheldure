using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Trains.Model.Entities;
using Windows.UI.Xaml.Media.Imaging;

namespace Trains.WP.Converter
{
    public class ShareSocialToImageConverter:IValueConverter
    {
        static readonly Dictionary<ShareSocial, Uri> Pictures =
    new Dictionary<ShareSocial, Uri>()
            {
                {ShareSocial.Vkontakte,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.vkontakte.png")},
                {ShareSocial.Facebook,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.social.facebook.heart.png")},
                {ShareSocial.Twitter,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.social.twitter.png")},
                {ShareSocial.GooglePlus,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.googleplus.png")},
                {ShareSocial.LinkedIn,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.social.linkedin.png")},
                {ShareSocial.Odnoklassniki,new Uri(@"ms-appx:///Assets/AppBarIcons/appbar.odnoklassniki.png")}
            };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new BitmapImage(Pictures[(ShareSocial)value]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
