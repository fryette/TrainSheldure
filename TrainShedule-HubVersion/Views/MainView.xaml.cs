using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Trains.App.Views
{
    public sealed partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            MyStoryboard.Begin();
            MyStoryboard1.Begin();
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
            UpdateAppBar.Visibility = updateAppBar?Visibility.Visible : Visibility.Collapsed;
            ManagedAppBar.Visibility = managedAppBar ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
