using Windows.UI.Xaml;
using Trains.Core.ViewModels;

namespace Trains.UAP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CarriageView
    {
        public CarriageView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
