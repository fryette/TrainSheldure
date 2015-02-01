using System.Threading.Tasks;
using TrainShedule_HubVersion.Common;
using TrainShedule_HubVersion.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using TrainShedule_HubVersion.DataModel;

namespace TrainShedule_HubVersion
{
    /// <summary>
    /// A page that displays details for a single item within a group.
    /// </summary>
    public sealed partial class ItemPage
    {
        private readonly NavigationHelper _navigationHelper;
        private readonly ObservableDictionary _defaultViewModel = new ObservableDictionary();

        public ItemPage()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
            _navigationHelper.SaveState += NavigationHelper_SaveState;
            InitializeAutoSuggestions();
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
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var item = await SampleDataSource.GetItemAsync((string)e.NavigationParameter);
            DefaultViewModel["Item"] = item;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
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

        private IEnumerable<string> _autoCompletions = null;

        private void FromTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;
            From.ItemsSource = AutoSuggestCity(sender.Text);
        }

        private void ToTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
                return;
            To.ItemsSource = AutoSuggestCity(sender.Text);
        }

        private List<string> AutoSuggestCity(string input)
        {
            if (input.Length < 2) return null;
            return _autoCompletions.Where(city => city.Contains(input)).ToList();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_autoCompletions.Contains(From.Text) || !_autoCompletions.Contains(To.Text))
            {
                var messageDialog = new MessageDialog("Один или оба пункта не существует");
                await messageDialog.ShowAsync();
                return;
            }
            var schedule = await GetTrainSchedure(From.Text, To.Text, GetDate());
            try
            {
                Frame.Navigate(typeof(Schedule), schedule);
            }
            catch (Exception)
            {
                // TODO: Writy implementation
            }
            finally
            {
                MyIndeterminateProbar.Visibility = Visibility.Collapsed;
            }
        }

        private Task<IEnumerable<Train>> GetTrainSchedure(string from, string to, string date)
        {
            MyIndeterminateProbar.Visibility = Visibility.Visible;
            return Task.Run(() => TrainGrabber.GetTrainSchedure(from, to, date));
        }
        private string GetDate()
        {
            return DatePicker.Date.Year + "-" + DatePicker.Date.Month + "-" + DatePicker.Date.Day;
        }

        private async void addBtn_Click(object sender, RoutedEventArgs e)
        {
            _autoCompletions = TrainPointsGrabber.GetTrainsPoints();
            await Serialize.SaveObjectToXml((List<string>)_autoCompletions, "autocompletetions");
        }

        private async void InitializeAutoSuggestions()
        {
            if (_autoCompletions != null) return;
            try
            {
                _autoCompletions = await Serialize.ReadObjectFromXmlFileAsync("autocompletetions");
            }
            catch (Exception e)
            {
                _autoCompletions = TrainPointsGrabber.GetTrainsPoints();
            }
            await Serialize.SaveObjectToXml((List<string>)_autoCompletions, "autocompletetions");
        }
    }
}
