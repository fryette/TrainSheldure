using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using TrainShedule_HubVersion.DataModel;

namespace TrainShedule_HubVersion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StopPointSchedule : Page
    {
        private IEnumerable<TrainStop> _trainStop;
        public StopPointSchedule()
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
            _trainStop = e.Parameter as IEnumerable<TrainStop>;
            if (_trainStop != null)
                TrainList.ItemsSource = _trainStop;
        }
    }
}
