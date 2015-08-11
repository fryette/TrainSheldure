using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Cirrious.CrossCore;
using Trains.Core.Interfaces;
using Trains.Model.Entities;

namespace Trains.Universal.Converter
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
            var image = "";
            if ((string)parameter == "route" && (value == null || !((IEnumerable<Route>)value).Any()))
                image = RoutesBackground[Mvx.Resolve<IAppSettings>().Language.Id];
            else if (value == null)
            {
                if ((string)parameter == "reverse")
                    image = ReverseBackground[Mvx.Resolve<IAppSettings>().Language.Id];
                else if ((string)parameter == "last")
                    image = LastScheduleRoute[Mvx.Resolve<IAppSettings>().Language.Id];
            }
            var color = (App.Current.Resources["SystemControlForegroundAccentBrush"] as SolidColorBrush).Color;
            return new ImageBrush
                 {
                     ImageSource = new BitmapImage
                     {
                         UriSource = new Uri(UriSource + image + ((color.R + color.G + color.B) / 3 > 127 ? "White.png" : "Black.png"))
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
