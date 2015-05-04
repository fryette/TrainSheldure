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
        static readonly Dictionary<string, Uri> Pictures =
   new Dictionary<string, Uri>()
            {
                {"ru",new Uri(@"ms-appx:///Assets/backgrounds/Dlya_Otobrazhenia.png")},
                {"be",new Uri(@"ms-appx:///Assets/backgrounds/Dlya_Adlyustravannya.png")},
                {"en",new Uri(@"ms-appx:///Assets/backgrounds/No_history.png")},
            };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (((MainViewModel)value).Trains != null) return null;
            return new ImageBrush
            {
                ImageSource = new BitmapImage() { UriSource = Pictures[Mvx.Resolve<IAppSettings>().Language.Id] },
                Stretch = Stretch.UniformToFill
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
