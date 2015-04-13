using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Trains.WP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            MyStoryboard.Begin();
        }

        private void Pivot_OnPivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            if (args.Item == RoutesPivot)
                SetAppBarVisibility(false, true);
            else if (args.Item == LastPivot)
                SetAppBarVisibility(true, false);
            else
                SetAppBarVisibility(false, false);
        }

        private void SetAppBarVisibility(bool updateAppBar, bool managedAppBar)
        {
            UpdateAppBar.Visibility = updateAppBar ? Visibility.Visible : Visibility.Collapsed;
            ManagedAppBar.Visibility = managedAppBar ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = false;
            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = true;
        }


        //private void KittyPic_ImageOpened(object sender, RoutedEventArgs e)
        //{
        //    LoadingPanel.Visibility = Visibility.Collapsed;
        //}

        //private async void KittyPic_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        //{
        //    LoadingPanel.Visibility = Visibility.Collapsed;
        //    await new MessageDialog("Failed to load the image").ShowAsync();
        //}



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
