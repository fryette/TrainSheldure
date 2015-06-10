using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Trains.UAP.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScheduleControl : Page
    {
        public ScheduleControl()
        {
            this.InitializeComponent();
        }

        private void TrainList_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            CommandButton.Command.Execute(TrainList.SelectedItem);
        }
    }
}
