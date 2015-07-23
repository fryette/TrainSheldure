using Windows.UI.Xaml.Controls;

namespace Trains.WP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpView
    {
        static int _lastPivotIndex;
        public HelpView()
        {
            InitializeComponent();
            MainPivot.SelectedIndex = _lastPivotIndex;
        }

        private void Pivot_OnPivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            _lastPivotIndex = MainPivot.SelectedIndex;
        }
    }
}
