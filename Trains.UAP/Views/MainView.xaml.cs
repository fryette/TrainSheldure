using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Trains.Core.Resources;
using Trains.Core.ViewModels;
using Trains.UAP.Controls;

namespace Trains.UAP.Views
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
            //MainPivot.SelectedIndex = LastPivotIndex;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Loaded += MainView_Loaded;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(MainControl));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof (ScheduleControl));
        }

        private void ButtonHome_OnClick(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof (MainControl));
        }

        private void ButtonFavorite_OnClick(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(FavoriteControl));
        }

        private void ButtonAbout_OnClick(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof (AboutControl));
        }
    }
}
