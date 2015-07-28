using System;
using Windows.ApplicationModel.Store;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Cirrious.MvvmCross.WindowsCommon.Views;
using Microsoft.Xaml.Interactions.Core;
using Trains.Core.ViewModels;
using Trains.UAP.Controls;

namespace Trains.UAP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainView
    {
        private static Type CurrentPage;

        public MainView()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            Loaded += MainView_Loaded;
        }

		private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateTo(CurrentPage ?? typeof(MainControl));
		}

		private void ButtonSchedule_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateTo(typeof(ScheduleControl));
        }

        private void ButtonHome_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateTo(typeof(MainControl));
        }

        private void ButtonFavorite_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateTo(typeof(FavoriteControl));
        }

        private void ButtonAbout_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateTo(typeof(AboutControl));
        }

        private void ButtonHelp_OnClick(object sender, RoutedEventArgs e)
        {
            NavigateTo(typeof(HelpControl));
        }

        private void NavigateTo(Type page)
        {
            rootFrame.Navigate(page);
            CurrentPage = page;
            OpenClosePane(false);
            ManageVisibilityAppBar();
        }

        private void ButtonOpen_OnClick(object sender, RoutedEventArgs e)
        {
            OpenClosePane(!rootSplitView.IsPaneOpen);
        }

        private void OpenClosePane(bool isPaneOpen)
        {
            rootSplitView.IsPaneOpen = isPaneOpen;
        }

        private void ManageVisibilityAppBar()
        {
            if (CurrentPage == typeof(MainControl))
                SetVisibility(true);
            else if(CurrentPage==typeof(ScheduleControl))
                SetVisibility(false,true);
            else
                SetVisibility();
        }

        private void SetVisibility(bool isSwapVisible=false, bool isUpdateVisibile=false)
        {
            //UpdateAppBar.Visibility = isUpdateVisibile ? Visibility.Visible : Visibility.Collapsed;
            //SwapAppBar.Visibility = isSwapVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
