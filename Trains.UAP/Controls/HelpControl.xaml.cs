using Windows.UI.Xaml.Controls;
using Cirrious.CrossCore;
using Trains.Core.Interfaces;
using Trains.Core.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Trains.UAP.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpControl
    {
        static int _lastPivotIndex;
        private static HelpViewModel _model;
        public HelpControl()
        {
            InitViewModel();
            DataContext = _model;
            this.InitializeComponent();
            MainPivot.SelectedIndex = _lastPivotIndex;
        }

        private void Pivot_OnPivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            _lastPivotIndex = MainPivot.SelectedIndex;
        }

        private void InitViewModel()
        {
            if (_model != null) return;
            _model = new HelpViewModel(Mvx.Resolve<IAppSettings>());
            _model.Init();
        }
    }
}
