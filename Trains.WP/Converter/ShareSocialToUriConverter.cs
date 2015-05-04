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
            if (Pictures == null)
            {
                var data = Mvx.Resolve<IAppSettings>().SocialUri;
                Pictures = new Dictionary<ShareSocial, Uri>()
            {
                {ShareSocial.Vkontakte,new Uri(data.Vkontakte)},
                {ShareSocial.Facebook,new Uri(data.Facebook)},
                {ShareSocial.Twitter,new Uri(data.Twitter)},
                {ShareSocial.GooglePlus,new Uri(data.GooglePlus)},
                {ShareSocial.LinkedIn,new Uri(data.Linkedin)},
                {ShareSocial.Odnoklassniki,new Uri(data.Odnoklassniki)}
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
