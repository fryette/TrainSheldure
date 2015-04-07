using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.WP.Converter
{
    class EnumToImagePathConverter : Windows.UI.Xaml.Data.IValueConverter
    {
        private static string[] Images = new string[]
        {
            "/Assets/Inteneshnl.png",
            "/Assets/Interregional_biznes.png",
            "/Assets/Interregional_econom.png",
            "/Assets/Regional_biznes.png",
            "/Assets/Regional_econom.png",
            "/Assets/Cityes.png",
            "/Assets/Foreign.png"
        };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Images[(int)(Picture)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
