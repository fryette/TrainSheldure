using Cirrious.CrossCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Core.Interfaces;
using Trains.Core.ViewModels;
using Trains.Entities;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Trains.WP.Converter
{
    public class BackgroundLastRouteToImageConverter : IValueConverter
    {
        static readonly Dictionary<string, Uri> LastScheduleRoute = new Dictionary<string, Uri>()
            {
                {"ru",new Uri(@"ms-appx:///Assets/backgrounds/Dlya_Otobrazhenia.png")},
                {"be",new Uri(@"ms-appx:///Assets/backgrounds/Dlya_Adlyustravannya.png")},
                {"en",new Uri(@"ms-appx:///Assets/backgrounds/No_history.png")},
            };
        static readonly Dictionary<string, Uri> RoutesBacground = new Dictionary<string, Uri>()
            {
                {"ru",new Uri(@"ms-appx:///Assets/backgrounds/Marshrutov_Ne.png")},
                {"be",new Uri(@"ms-appx:///Assets/backgrounds/Marshrutau_Ne.png")},
                {"en",new Uri(@"ms-appx:///Assets/backgrounds/No_favorites.png")},
            };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new ImageBrush
            {
                ImageSource = parameter == null ? (value != null ? null : new BitmapImage() { UriSource = LastScheduleRoute[Mvx.Resolve<IAppSettings>().Language.Id] }) :
                    (value != null ? null : new BitmapImage() { UriSource = RoutesBacground[Mvx.Resolve<IAppSettings>().Language.Id] }),
                Stretch = Stretch.UniformToFill
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
