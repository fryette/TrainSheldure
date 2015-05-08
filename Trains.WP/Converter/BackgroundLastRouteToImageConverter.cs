using Cirrious.CrossCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Core.Interfaces;
using Trains.Core.ViewModels;
using Trains.Entities;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Trains.WP.Converter
{
    public class BackgroundLastRouteToImageConverter : IValueConverter
    {
        static readonly Dictionary<string, string> LastScheduleRoute = new Dictionary<string, string>()
            {
                {"ru","ms-appx:///Assets/backgrounds/Dlya_Otobrazhenia"},
                {"be","ms-appx:///Assets/backgrounds/Dlya_Adlyustravannya"},
                {"en","ms-appx:///Assets/backgrounds/No_history"}
            };
        static readonly Dictionary<string, string> RoutesBackground = new Dictionary<string, string>()
            {
                {"ru","ms-appx:///Assets/backgrounds/Marshrutov_Ne"},
                {"be","ms-appx:///Assets/backgrounds/Marshrutau_Ne"},
                {"en","ms-appx:///Assets/backgrounds/No_favorites"}
            };
        static readonly Dictionary<string, string> ReverseBackground = new Dictionary<string, string>()
            {
                {"ru","ms-appx:///Assets/backgrounds/Obratnyy"},
                {"be","ms-appx:///Assets/backgrounds/Zvarotny"},
                {"en","ms-appx:///Assets/backgrounds/Back"}
            };

        static readonly Dictionary<string, string> MainBackground = new Dictionary<string, string>()
            {
                {"ru","ms-appx:///Assets/backgrounds/Naydi"},
                {"be","ms-appx:///Assets/backgrounds/Znaydzi"},
                {"en","ms-appx:///Assets/backgrounds/Found"}
            };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = ((App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush).Color).R == 0 ? "Black.png" : "White.png";

            string UriSource = null;
            if ((string)parameter == "main")
                return new ImageBrush
                {
                    ImageSource = new BitmapImage
                    {
                        UriSource = new Uri(MainBackground[Mvx.Resolve<IAppSettings>().Language.Id] + color)
                    },
                    Stretch = Stretch.UniformToFill
                };
            if (value == null)
            {
                if ((string)parameter == "reverse")
                    UriSource = ReverseBackground[Mvx.Resolve<IAppSettings>().Language.Id];
                else if ((string)parameter == "last")
                    UriSource = LastScheduleRoute[Mvx.Resolve<IAppSettings>().Language.Id];
                else if ((string)parameter == "route")
                    UriSource = RoutesBackground[Mvx.Resolve<IAppSettings>().Language.Id];
                return new ImageBrush
                     {
                         ImageSource = new BitmapImage
                         {
                             UriSource = new Uri(UriSource + color)
                         },
                         Stretch = Stretch.UniformToFill
                     };
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
