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
        }

        private void TrainList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var c = CommandClick.Command;
            c.Execute(TrainList.SelectedItem);
        }

    }
}
