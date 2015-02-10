using System;
using TrainShedule_HubVersion.Common;
using TrainShedule_HubVersion.DataModel;
using Windows.Graphics.Display;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TrainShedule_HubVersion
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage
    {
        private readonly NavigationHelper _navigationHelper;
        private readonly ObservableDictionary _defaultViewModel = new ObservableDictionary();
        private MenuDataItem _item;
        public HubPage()
        {
            InitializeComponent();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            NavigationCacheMode = NavigationCacheMode.Required;
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
        }
        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return _defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var sampleDataGroups = await MenuDataSource.GetGroupsAsync();
            DefaultViewModel["Groups"] = sampleDataGroups;
            SetLastTrainSchedule();
        }

        private void Hub_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            SetLastTrainSchedule();
        }

        private async void SetLastTrainSchedule()
        {
            var listview = Helper.FindChildControl<ListView>(this, "TrainList") as ListView;
            if (listview == null) return;
            var lastTrainSchedule = await Serialize.ReadObjectFromXmlFileAsync<Train>("LastTrainList");
            listview.ItemsSource = lastTrainSchedule;
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _item = e.ClickedItem as MenuDataItem;
            if (_item != null && (_item.UniqueId == "Menu-Interregional" || _item.UniqueId == "Menu-Regional"))
                Frame.Navigate(typeof(SectionPage), _item);
            else Frame.Navigate(typeof (ItemPage), _item);
        }

        #region NavigationHelper registration
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
