namespace Trains.UAP.Views
{
    public sealed partial class InformationView
    {
        public InformationView()
        {
            InitializeComponent();
        }

        private void ButtonBack_OnClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
