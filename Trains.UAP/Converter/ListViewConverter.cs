using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;
using Trains.Entities;

namespace Trains.UAP.Converter
{
    public class ListViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return null;
            CollectionViewSource collection = new CollectionViewSource
            {
                Source = ((List<Train>)value).GroupBy(x => x.BeforeDepartureTime),
                IsSourceGrouped = true
            };
            return collection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
