using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Cirrious.MvvmCross.WindowsCommon.Views;
using Trains.UAP.Controls;

namespace Trains.UAP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainView
    {
        public static Frame RootFrame;

        public MainView()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            Loaded += MainView_Loaded;
            RootFrame = rootFrame;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(MainControl));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(ScheduleControl));
        }

        private void ButtonHome_OnClick(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(MainControl));
        }

        private void ButtonFavorite_OnClick(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(FavoriteControl));
        }

        private void ButtonAbout_OnClick(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(AboutControl));
        }

        private void ButtonHelp_OnClick(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(HelpControl));
        }
    }
}
