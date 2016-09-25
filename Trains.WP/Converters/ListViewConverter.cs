using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;
using Trains.Model;

namespace Trains.WP.Converters
{
	public class ListViewConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value == null) return null;
			var collection = new CollectionViewSource
			{
				Source = ((List<TrainModel>)value).GroupBy(x => x.Time.StartTime),
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
