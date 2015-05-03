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
    public class ShareSocialToUriConverter:IValueConverter
    {
        static readonly Dictionary<ShareSocial, Uri> Pictures =
    new Dictionary<ShareSocial, Uri>()
            {
                {ShareSocial.Vkontakte,new Uri("https://vk.com/share.php?url=https://vk.com/chygunka?w=wall-93365252_2&title=Chygunka.by%20на%20Windows%20Phone!&description=Все%20расписание%20поездов%20в%20твоем%20windows%20phone!А%20скоро%20и%20Android!&image=https://pp.vk.me/c624330/v624330780/360b3/RfJxmzA5y0U.jpg&noparse=true")},
                {ShareSocial.Facebook,new Uri("https://www.facebook.com/sharer/sharer.php?u=https://www.facebook.com/permalink.php?story_fbid=1412539452400696%26id=100009339649714%26pnref=story")},
                {ShareSocial.Twitter,new Uri("https://twitter.com/home?status=%23Chygunka%20%D0%BD%D0%B0%20Windows%20Phone!%0A%D0%9F%D1%80%D0%B8%D0%BB%D0%BE%D0%B6%D0%B5%D0%BD%D0%B8%D0%B5%20%D0%B4%D0%BB%D1%8F%20%D0%BF%D0%BE%D0%B8%D1%81%D0%BA%D0%B0%20%D1%80%D0%B0%D1%81%D0%BF%D0%B8%D1%81%D0%B0%D0%BD%D0%B8%D1%8F%20%D0%BF%D0%BE%D0%B5%D0%B7%D0%B4%D0%BE%D0%B2%20%D0%B2%D1%8B%D1%88%D0%BB%D0%BE%0A%D1%81%D1%81%D1%8B%D0%BB%D0%BA%D0%B0%20%D0%BD%D0%B0%20WP%20https://www.windowsphone.com/ru-RU/store/app/%25D1%2587%25D1%258B%25D0%25B3%25D1%2583%25D0%25BD%25D0%25BA%25D0%25B0/9a0879a6-0764-4e99-87d7-4c1c33f2d78e%0A%D0%A1%D0%B0%D0%B9%D1%82:%20http://chygunka.by")},
                {ShareSocial.GooglePlus,new Uri("https://plus.google.com/share?url=https://plus.google.com/107562493460823748217/posts/22YdFP7ftdN")},
                {ShareSocial.LinkedIn,new Uri("https://www.linkedin.com/shareArticle?mini=true&url=https://www.linkedin.com/groups/%25D0%2592%25D1%258B%25D1%2585%25D0%25BE%25D0%25B4-%25D0%25B2-Windows-Phone-Store-8297750.S.6000682329939410947?trk=groups_most_recent-0-b-ttl%26goback=%252Egmp_8297750%252Egmr_8297750&title=Chygunka.by%20%D0%BD%D0%B0%20Windows%20Phone!&summary=%D0%92%D1%81%D0%B5%20%D1%80%D0%B0%D1%81%D0%BF%D0%B8%D1%81%D0%B0%D0%BD%D0%B8%D0%B5%20%D0%BF%D0%BE%D0%B5%D0%B7%D0%B4%D0%BE%D0%B2%20%D0%B2%20%D1%82%D0%B2%D0%BE%D0%B5%D0%BC%20windows%20phone!%D0%90%20%D1%81%D0%BA%D0%BE%D1%80%D0%BE%20%D0%B8%20Android!&source=")},
                {ShareSocial.Odnoklassniki,new Uri("http://www.odnoklassniki.ru/dk?st.cmd=addShare&st.s=1&st._surl=http://ok.ru/group/57389300777013/topic/63791795492149")}
            };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Pictures[(ShareSocial) value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
