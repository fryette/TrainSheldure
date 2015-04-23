namespace Trains.WP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpView
    {
        static int LastPivotIndex;
        public HelpView()
        {
            InitializeComponent();
            MainPivot.SelectedIndex = LastPivotIndex;
        }

        private void Pivot_OnPivotItemLoaded(Windows.UI.Xaml.Controls.Pivot sender, Windows.UI.Xaml.Controls.PivotItemEventArgs args)
        {
            LastPivotIndex = MainPivot.SelectedIndex;
        }
    }
}
