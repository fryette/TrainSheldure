using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Cirrious.CrossCore;
using Trains.Core.Resources;
using Trains.Core.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Trains.UAP.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainControl
    {
        public MainControl()
        {
            this.InitializeComponent();
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = false;
            CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = true;
            SetVisibility(Visibility.Visible);
            SetVisibilityAutossugestBox(Visibility.Visible, Visibility.Visible);
            comboBox_SelectionChanged(null, null);
        }

        private void AutoSuggestBox_ManipulationStarted(object sender, RoutedEventArgs e)
        {
            SetVisibility(Visibility.Collapsed);
            DataPicker.Visibility = Visibility.Collapsed;
            if ((AutoSuggestBox)sender == From)
                SetVisibilityAutossugestBox(Visibility.Visible, Visibility.Collapsed);
            else
                SetVisibilityAutossugestBox(Visibility.Collapsed, Visibility.Visible);

        }

        private void AutoSuggestBox_ManipulationCompleted(object sender, RoutedEventArgs e)
        {
            SetVisibility(Visibility.Visible);
            SetVisibilityAutossugestBox(Visibility.Visible, Visibility.Visible);
            comboBox_SelectionChanged(null, null);
        }

        private void SetVisibility(Visibility visibility)
        {
            comboBox.Visibility = visibility;
            SearchButton.Visibility = visibility;
            Routes.Visibility = visibility;
        }

        void SetVisibilityAutossugestBox(Visibility from, Visibility to)
        {
            From.Visibility = from;
            To.Visibility = to;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataPicker.Visibility = comboBox.SelectedItem == ResourceLoader.Instance.Resource["OnDay"] ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
