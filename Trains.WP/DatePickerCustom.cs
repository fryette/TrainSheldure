using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Trains.WP
{
    public class DatePickerCustom : DatePicker
    {
        protected override void OnTapped(Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
        }

        public void OnTapped()
        {
            base.OnTapped(new Windows.UI.Xaml.Input.TappedRoutedEventArgs());

        }
    }
}
