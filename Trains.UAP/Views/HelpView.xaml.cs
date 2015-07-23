using Windows.UI.Xaml.Controls;

namespace Trains.UAP.Views
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

        private void Pivot_OnPivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            LastPivotIndex = MainPivot.SelectedIndex;
        }
    }
}
