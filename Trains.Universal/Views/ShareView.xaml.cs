
namespace Trains.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShareView
    {
        public ShareView()
        {
            this.InitializeComponent();
        }
        private void ButtonBack_OnClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
