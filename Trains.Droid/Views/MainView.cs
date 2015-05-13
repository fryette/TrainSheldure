using Android.App;
using Android.OS;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System;
using System.Collections.Generic;
using Trains.Core.ViewModels;

namespace Trains.Droid.Views
{
    [Activity(Label = "MainView")]
    public class MainView : MvxActivity
    {
        private const string DateFormat = "d";

        private Button _searchDateButton;
        private Button _searchTypeButton;
        private AutoCompleteTextView _fromTextView;
        private AutoCompleteTextView _toTextView;

        private DateTimeOffset SearchDate
        {
            get { return ((MainViewModel)ViewModel).Datum; }
            set { ((MainViewModel)ViewModel).Datum = value; }
        }

        private List<string> SearchTypes
        {
            get { return ((MainViewModel)ViewModel).VariantOfSearch; }
        }

        private string SelectedType
        {
            get { return ((MainViewModel)ViewModel).SelectedVariant; }
            set { ((MainViewModel)ViewModel).SelectedVariant = value; }
        }

        private List<string> AutoCompletion
        {
            get { return ((MainViewModel)ViewModel).AutoSuggestions; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);

            _searchDateButton = FindViewById<Button>(Resource.Id.SearchDate);
            _searchTypeButton = FindViewById<Button>(Resource.Id.SearchType);
            _fromTextView = FindViewById<AutoCompleteTextView>(Resource.Id.FromTextView);
            _toTextView = FindViewById<AutoCompleteTextView>(Resource.Id.ToTextView);
        }

        protected override void OnStart()
        {
            _searchDateButton.Click += searchDateButton_Click;
            _searchDateButton.Text = SearchDate.ToString(DateFormat);

            _searchTypeButton.Click += searchTypeButton_Click;
            _searchTypeButton.Text = SelectedType;
			_fromTextView.TextChanged += fromTextView_TextChange;
			_toTextView.TextChanged += toTextView_TextChange;

            base.OnStart();
        }

		private void fromTextView_TextChange(object sender, EventArgs e)
		{
			_fromTextView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, AutoCompletion);
		}
			
		private void toTextView_TextChange(object sender, EventArgs e)
		{
			_toTextView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, AutoCompletion);
		}

        private void searchDateButton_Click(object sender, EventArgs e)
        {
            ShowDialog((int)DialogTypes.DatePicker);
        }

        private void searchTypeButton_Click(object sender, EventArgs e)
        {
            ShowDialog((int)DialogTypes.SearchType);
        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case (int)DialogTypes.DatePicker:
                    return new DatePickerDialog(this, HandleSearchDateSet, SearchDate.Year, SearchDate.Month - 1, SearchDate.Day);
                case (int)DialogTypes.SearchType:
                    var dialog = new AlertDialog.Builder(this).Create();
                    var listView = new ListView(this);
                    listView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SelectDialogItem, SearchTypes);
                    listView.ItemClick += (sender, args) =>
                    {
                        SelectedType = SearchTypes[args.Position];
                        _searchTypeButton.Text = SelectedType;

                        dialog.Cancel();
                    };
                    dialog.SetView(listView);
                    return dialog;
                default:
                    return base.OnCreateDialog(id);
            }
        }

        private void HandleSearchDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            SearchDate = new DateTimeOffset(e.Date);
            _searchDateButton.Text = e.Date.ToString(DateFormat);
        }
    }

    enum DialogTypes : byte
    {
        DatePicker = 0,
        SearchType = 1
    }
}