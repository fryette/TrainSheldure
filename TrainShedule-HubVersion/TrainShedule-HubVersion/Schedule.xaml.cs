using System.Collections.Generic;
using System.Linq;
using TrainShedule_HubVersion.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TrainShedule_HubVersion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Schedule
    {
        List<Train> _trainList;
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
            _trainList = e.Parameter as List<Train>;
        }

        void SetTrainSheldure(object sender, RoutedEventArgs e)
        {
            TrainList.ItemsSource = _trainList.Where(x=>x.Status=="True");
        }
    }
}
