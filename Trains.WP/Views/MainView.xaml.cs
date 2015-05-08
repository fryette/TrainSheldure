using Trains.Core.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Trains.WP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView
    {
        static int LastPivotIndex;
        public MainView()
        {
            InitializeComponent();
            MainPivot.SelectedIndex = LastPivotIndex;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void Pivot_OnPivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            LastPivotIndex = MainPivot.SelectedIndex;
            if (args.Item == MainPivotItem) SetAppBarVisibility(false, false, true);
            else if (args.Item == RoutesPivot)
                SetAppBarVisibility(false, true);
            else if (args.Item == LastPivot)
                SetAppBarVisibility(true);
            else
                SetAppBarVisibility();

            ((MainViewModel)ViewModel).RaisePropertyChanged("LastUpdateTime");
        }

        private void SetAppBarVisibility(bool updateAppBar = false, bool managedAppBar = false, bool swapAppBar = false)
        {
            UpdateAppBar.Visibility = updateAppBar ? Visibility.Visible : Visibility.Collapsed;
            ManagedAppBar.Visibility = managedAppBar ? Visibility.Visible : Visibility.Collapsed;
            SwapAppBar.Visibility = swapAppBar ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = false;
            CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = true;
        }


        private void TrainList_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            CommandButton.Command.Execute(TrainList.SelectedItem);
        }
    }
}
