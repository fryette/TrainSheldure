using Cirrious.CrossCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Core.Interfaces;
using Trains.Core.ViewModels;
using Trains.Entities;
using Trains.Model.Entities;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Trains.WP.Converter
{
    public class BackgroundLastRouteToImageConverter : IValueConverter
    {
        private const string UriSource = "ms-appx:///Assets/backgrounds/";
        static readonly Dictionary<string, string> LastScheduleRoute = new Dictionary<string, string>()
            {
                {"ru","Dlya_Otobrazhenia"},
                {"be","Dlya_Adlyustravannya"},
                {"en","No_history"}
            };
        static readonly Dictionary<string, string> RoutesBackground = new Dictionary<string, string>()
            {
                {"ru","Marshrutov_Ne"},
                {"be","Marshrutau_Ne"},
                {"en","No_favorites"}
            };
        static readonly Dictionary<string, string> ReverseBackground = new Dictionary<string, string>()
            {
                {"ru","Obratnyy"},
                {"be","Zvarotny"},
                {"en","Back"}
            };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = ((App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush).Color).R == 0 ? "Black.png" : "White.png";
            var image = "";
            if (value == null)
            {
                if ((string)parameter == "reverse")
                    image = ReverseBackground[Mvx.Resolve<IAppSettings>().Language.Id];
                else if ((string)parameter == "last")
                    image = LastScheduleRoute[Mvx.Resolve<IAppSettings>().Language.Id];
            }
            else if ((string)parameter == "route" && (value == null || !((List<LastRequest>)value).Any()))
                image = RoutesBackground[Mvx.Resolve<IAppSettings>().Language.Id];
            return new ImageBrush
                 {
                     ImageSource = new BitmapImage
                     {
                         UriSource = new Uri(UriSource + image + color)
                     },
                     Stretch = Stretch.UniformToFill
                 };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
