using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Trains.Core.ViewModels;

namespace Trains.WP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScheduleView
    {
        public ScheduleView()
        {
            InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void TrainList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (((ScheduleViewModel)ViewModel).IsSearchStart) return;
            CommandClick.Command?.Execute(TrainList.SelectedItem);
        }

		private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
		{
			var senderElement = sender as FrameworkElement;
			var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
			flyoutBase.ShowAt(senderElement);
		}
	}
}
