﻿using System;
using Windows.UI.Xaml.Data;

namespace Trains.WP.Converter
{
    public class SelectionChangedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
