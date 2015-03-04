using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Microsoft.Xaml.Interactivity;
using TrainShedule_HubVersion.Entities;
using TrainShedule_HubVersion.ViewModels;

namespace TrainShedule_HubVersion.Infrastructure
{
    public class OpenMenuFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);

            flyoutBase.ShowAt(senderElement);

            return null;
        }
        private void SelectItem()
        {
        }
    }
}
