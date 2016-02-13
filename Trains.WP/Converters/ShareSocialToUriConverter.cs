using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;
using Cirrious.CrossCore;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Model.Entities;

namespace Trains.WP.Converters
{
	public class ShareSocialToUriConverter : IValueConverter
	{
		static Dictionary<ShareSocial, Uri> _pictures;

		public ShareSocialToUriConverter()
		{
			var data = Mvx.Resolve<IAppSettings>();
			if (_pictures == null)
			{
				_pictures = new Dictionary<ShareSocial, Uri>()
			{
				{ShareSocial.VKONTAKTE,new Uri(data.SocialUri.Vkontakte)},
				{ShareSocial.FACEBOOK,new Uri(data.SocialUri.Facebook)},
				{ShareSocial.TWITTER,new Uri(data.SocialUri.Twitter)},
				{ShareSocial.GOOGLEPLUS,new Uri(data.SocialUri.GooglePlus)},
				{ShareSocial.LINKEDIN,new Uri(data.SocialUri.Linkedin)},
				{ShareSocial.ODNOKLASSNIKI,new Uri(data.SocialUri.Odnoklassniki)}
			};
			}
		}

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return _pictures[(ShareSocial)value];
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
