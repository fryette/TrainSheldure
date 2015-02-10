﻿using TrainShedule_HubVersion.Common;
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
        private MenuDataItem _item;
        private IEnumerable<string> _autoCompletion;

        public ItemPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
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
            _item = e.NavigationParameter as MenuDataItem;
            DefaultViewModel["Item"] = _item;
            if (_item != null) CheckItemForAirport(_item.UniqueId);
            if (_autoCompletion == null)
                _autoCompletion = await Serialize.ReadObjectFromXmlFileAsync<string>("autocompletion");
        }

        private void CheckItemForAirport(string uniqueId)
        {
            if (uniqueId == "Menu-Airport")
                From.Text = "Национальный аэропорт «Минск»";
        }

        private void AutoSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                sender.ItemsSource = sender.Text.Length < 2
                    ? null
                    : _autoCompletion.Where(city => city.Contains(sender.Text)).ToList();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (_autoCompletion == null && (!_autoCompletion.Contains(From.Text) || !_autoCompletion.Contains(To.Text)))
            {
                var messageDialog = new MessageDialog("Один или оба пункта не существует, проверьте или обновите пункты");
                await messageDialog.ShowAsync();
                return;
            }
            MyIndeterminateProbar.Visibility = Visibility.Visible;
            var schedule = await TrainGrabber.GetTrainSchedule(From.Text, To.Text, GetDate(), _item.Title);
            Frame.Navigate(typeof(Schedule), schedule);
            MyIndeterminateProbar.Visibility = Visibility.Collapsed;
        }

        private async void UpdateTrainStop_Click(object sender, RoutedEventArgs e)
        {
            _autoCompletion = TrainPointsGrabber.GetTrainPoints();
            await Serialize.SaveObjectToXml(new List<string>(_autoCompletion), "autocompletion");
        }

        private void Swap(object sender, RoutedEventArgs e)
        {
            var temp = From.Text;
            From.Text = To.Text;
            To.Text = temp;
        }

        private string GetDate()
        {
            return DatePicker.Date.Year + "-" + DatePicker.Date.Month + "-" + DatePicker.Date.Day;
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