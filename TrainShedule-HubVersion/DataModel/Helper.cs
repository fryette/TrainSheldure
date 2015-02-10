using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace TrainShedule_HubVersion.DataModel
{
    class Helper
    {
        public static DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            var childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (var i = 0; i < childNumber; i++)
            {
                var child = VisualTreeHelper.GetChild(control, i);
                var fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                // Not found it - search children
                var nextLevel = FindChildControl<T>(child, ctrlName);
                if (nextLevel != null)
                    return nextLevel;
            }
            return null;
        }
    }
}
