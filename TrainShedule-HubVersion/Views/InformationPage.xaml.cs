using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using TrainShedule_HubVersion.DataModel;
using TrainShedule_HubVersion.Infrastructure;

namespace TrainShedule_HubVersion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InformationPage
    {
        private Train _train;
        public InformationPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _train = e.Parameter as Train;
            if (_train != null)
                TrainList.ItemsSource = _train.AdditionalInformation;
        }

        private async void SearchStopPoint(object sender, RoutedEventArgs e)
        {
            MyIndeterminateProbar.Visibility = Visibility.Visible;
            var stopPointList =await Task.Run(() => AllTrainStop.GetTrainStop(_train.City.Substring(0, _train.City.IndexOf(" ", System.StringComparison.Ordinal)), _train.DepartureDate));
            MyIndeterminateProbar.Visibility = Visibility.Collapsed;
            Frame.Navigate(typeof(StopPointSchedule), stopPointList);
        }
    }
}