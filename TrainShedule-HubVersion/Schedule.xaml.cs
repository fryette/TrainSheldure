using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainShedule_HubVersion.DataModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace TrainShedule_HubVersion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Schedule
    {
        IEnumerable<Train> _trainList;
        public Schedule()
        {
            InitializeComponent();
            Loaded += SetTrainSheldure;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _trainList = (e.Parameter as IEnumerable<Train>).Where(x => !x.BeforeDepartureTime.Contains('-'));
        }

        void SetTrainSheldure(object sender, RoutedEventArgs e)
        {
            TrainList.ItemsSource = _trainList;
            Task.Run(() => Serialize.SaveObjectToXml(new List<Train>(_trainList), "LastTrainList"));
        }
    }
}
