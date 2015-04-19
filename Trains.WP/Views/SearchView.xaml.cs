using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Controls;

namespace Trains.WP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchView
    {
        public SearchView()
        {
            InitializeComponent();
        }
        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = false;
            CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = true;
        }
    }
}
