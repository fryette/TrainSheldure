﻿using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
            CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = false;
            CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = true;
        }


        private void TrainList_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            CommandButton.Command.Execute(TrainList.SelectedItem);
        }
    }
}
