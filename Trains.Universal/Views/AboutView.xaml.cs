namespace Trains.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutView
    {
        public AboutView()
        {
            InitializeComponent();
            MyStoryboard.Begin();
        }

        private void ButtonBack_OnClick(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
