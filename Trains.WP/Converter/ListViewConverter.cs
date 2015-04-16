using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Entities;
using Windows.UI.Xaml.Data;

namespace Trains.WP.Converter
{
   public class ListViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            CollectionViewSource collection = new CollectionViewSource();
            collection.Source = ((List<Train>)value).GroupBy(x=>x.BeforeDepartureTime);
            collection.IsSourceGrouped = true;
            return collection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
