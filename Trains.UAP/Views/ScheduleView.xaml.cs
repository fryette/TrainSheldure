using Windows.UI.Xaml.Input;
using Trains.Core.ViewModels;

namespace Trains.UAP.Views
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
            if (((ScheduleViewModel)ViewModel).IsSearchStart) return;
            CommandClick.Command.Execute(TrainList.SelectedItem);
        }
    }
}
