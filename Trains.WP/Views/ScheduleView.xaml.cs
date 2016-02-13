using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Trains.Core.ViewModels;

namespace Trains.WP.Views
{
    public sealed partial class ScheduleView
    {
        public ScheduleView()
        {
            InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void TrainListTapped(object sender, TappedRoutedEventArgs e)
        {
            if (((ScheduleViewModel)ViewModel).IsSearchStart) return;
            CommandClick.Command?.Execute(TrainList.SelectedItem);
        }

		private void GridHolding(object sender, HoldingRoutedEventArgs e)
		{
			var senderElement = sender as FrameworkElement;
			var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
			flyoutBase.ShowAt(senderElement);
		}
	}
}
