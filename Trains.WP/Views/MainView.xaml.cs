﻿using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Trains.Core.Resources;
using Trains.Core.ViewModels;

namespace Trains.WP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView
    {
        static int _lastPivotIndex;
        public MainView()
        {
            InitializeComponent();
            MainPivot.SelectedIndex = _lastPivotIndex;
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void Pivot_OnPivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            _lastPivotIndex = MainPivot.SelectedIndex;
            if (args.Item == MainPivotItem) SetAppBarVisibility(false, true);
            else if (args.Item == LastPivot)
                SetAppBarVisibility(true);
            else
                SetAppBarVisibility();

            ((MainViewModel)ViewModel).RaisePropertyChanged("LastUpdateTime");
        }

        private void SetAppBarVisibility(bool updateAppBar = false, bool swapAppBar = false)
        {
            UpdateAppBar.Visibility = updateAppBar ? Visibility.Visible : Visibility.Collapsed;
            SwapAppBar.Visibility = swapAppBar ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = false;
            CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = true;
            SetVisibility(Visibility.Visible);
            SetVisibilityAutossugestBox(Visibility.Visible, Visibility.Visible);
            ComboBox_SelectionChanged(null, null);
        }


        private void TrainList_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            CommandButton.Command?.Execute(TrainList.SelectedItem);
        }

        private void AutoSuggestBox_ManipulationStarted(object sender, RoutedEventArgs e)
        {
            SetVisibility(Visibility.Collapsed);
            DataPicker.Visibility = Visibility.Collapsed;
            if (sender as AutoSuggestBox == From)
                SetVisibilityAutossugestBox(Visibility.Visible, Visibility.Collapsed);
            else
                SetVisibilityAutossugestBox(Visibility.Collapsed, Visibility.Visible);

        }

        private void AutoSuggestBox_ManipulationCompleted(object sender, RoutedEventArgs e)
        {
            SetVisibility(Visibility.Visible);
            SetVisibilityAutossugestBox(Visibility.Visible, Visibility.Visible);
            ComboBox_SelectionChanged(null, null);
        }

        private void SetVisibility(Visibility visibility)
        {
            comboBox.Visibility = visibility;
            SearchButton.Visibility = visibility;
            Routes.Visibility = visibility;
	        CommandBar.Visibility = visibility;
        }

        void SetVisibilityAutossugestBox(Visibility from, Visibility to)
        {
            From.Visibility = from;
            To.Visibility = to;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataPicker.Visibility = ReferenceEquals(comboBox.SelectedItem, ResourceLoader.Instance.Resource["OnDay"]) ? Visibility.Visible : Visibility.Collapsed;
        }

		private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
		{
			var senderElement = sender as FrameworkElement;
			var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
			flyoutBase.ShowAt(senderElement);
		}
	}
}
