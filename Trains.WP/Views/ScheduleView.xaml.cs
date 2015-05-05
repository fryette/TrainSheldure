using Windows.UI.Xaml.Input;

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
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void TrainList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CommandClick.Command.Execute(TrainList.SelectedItem);
        }

    }
}
