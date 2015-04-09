using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Model.Entities;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Trains.WP.Converter
{
    class EnumToImagePathConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        //private static string[] Images = new string[]
        //{
        //    "/Assets/Inteneshnl.png",
        //    "/Assets/Interregional_biznes.png",
        //    "/Assets/Interregional_econom.png",
        //    "/Assets/Regional_biznes.png",
        //    "/Assets/Regional_econom.png",
        //    "/Assets/Cityes.png",
        //    "/Assets/Foreign.png"
        //};

        private static Color[] Images = new Color[]{
            Colors.Yellow,
            Colors.Green,
            Colors.Green,
            Colors.CadetBlue,
            Colors.CadetBlue,
            Colors.MediumVioletRed,
        };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new SolidColorBrush(Images[(int)(Picture)value]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
