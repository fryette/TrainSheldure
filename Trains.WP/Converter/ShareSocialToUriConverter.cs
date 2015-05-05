using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Trains.Model.Entities;
using Windows.UI.Xaml.Media.Imaging;
using Cirrious.CrossCore;
using Trains.Core.Interfaces;

namespace Trains.WP.Converter
{
    public class ShareSocialToUriConverter : IValueConverter
    {
        static Dictionary<ShareSocial, Uri> Pictures;

        public ShareSocialToUriConverter()
        {
            var data = Mvx.Resolve<IAppSettings>();
            if (Pictures == null)
            {
                Pictures = new Dictionary<ShareSocial, Uri>()
            {
                {ShareSocial.Vkontakte,new Uri(data.SocialUri.Vkontakte)},
                {ShareSocial.Facebook,new Uri(data.SocialUri.Facebook)},
                {ShareSocial.Twitter,new Uri(data.SocialUri.Twitter)},
                {ShareSocial.GooglePlus,new Uri(data.SocialUri.GooglePlus)},
                {ShareSocial.LinkedIn,new Uri(data.SocialUri.Linkedin)},
                {ShareSocial.Odnoklassniki,new Uri(data.SocialUri.Odnoklassniki)}
            };
            }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var b = new Uri(Mvx.Resolve<IAppSettings>().SocialUri.Vkontakte);
            var c = b;
            return Pictures[(ShareSocial)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
