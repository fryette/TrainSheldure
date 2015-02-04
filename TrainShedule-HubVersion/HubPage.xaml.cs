using System;
using System.Collections.Generic;
using TrainShedule_HubVersion.Common;
using TrainShedule_HubVersion.Data;
using TrainShedule_HubVersion.DataModel;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TrainShedule_HubVersion
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private readonly NavigationHelper _navigationHelper;
        private readonly ObservableDictionary _defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForCurrentView(@"Resources");
        public HubPage()
        {
            InitializeComponent();
            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            NavigationCacheMode = NavigationCacheMode.Required;

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
            _navigationHelper.SaveState += NavigationHelper_SaveState;
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
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            DefaultViewModel["Groups"] = sampleDataGroups;
            SetLastTrainSchedule();
        }

        private void Hub_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            SetLastTrainSchedule();
        }

        private async void SetLastTrainSchedule()
        {
            ListView listview = FindChildControl<ListView>(this, "TrainList") as ListView;
            if (listview == null) return;
            var lastTrainSchedule = await Serialize.ReadObjectFromXmlFileAsync<Train>("LastTrainList");
            listview.ItemsSource = lastTrainSchedule;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(_resourceLoader.GetString(@"NavigationFailedExceptionMessage"));
            }
        }
        private static DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            var childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (var i = 0; i < childNumber; i++)
            {
                var child = VisualTreeHelper.GetChild(control, i);
                var fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                // Not found it - search children
                var nextLevel = FindChildControl<T>(child, ctrlName);
                if (nextLevel != null)
                    return nextLevel;
            }
            return null;
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
