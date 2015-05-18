using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System;
using System.Collections.Generic;
using Trains.Core.ViewModels;
using Android.Views;
using System.Threading.Tasks;

namespace Trains.Droid.Views
{
	[Activity(Label = "View for MainViewModel")]
	public class MainView : MvxTabActivity
	{
        private const string DateFormat = "d";

        private Button _searchDateButton;
		private Button _searchTrainButton;
        private Button _searchTypeButton;
        private AutoCompleteTextView _fromTextView;
        private AutoCompleteTextView _toTextView;
		private ProgressBar _progressBar;

		private MainViewModel Model
		{
			get{ return (MainViewModel)ViewModel;}
		}

        private DateTimeOffset SearchDate
        {
            get { return Model.Datum; }
            set { Model.Datum = value; }
        }

        private List<string> SearchTypes
        {
            get { return Model.VariantOfSearch; }
        }

        private string SelectedType
        {
            get { return Model.SelectedVariant; }
            set { Model.SelectedVariant = value; }
        }

        private List<string> AutoCompletion
        {
            get { return Model.AutoSuggestions; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);

			TabHost.TabSpec spec;

			spec = TabHost.NewTabSpec("child1");
			spec.SetIndicator(Model.MainPivotItem);
            spec.SetContent(Resource.Id.tab1);
            TabHost.AddTab(spec);

            spec = TabHost.NewTabSpec("child2");
			spec.SetIndicator(Model.LastSchedulePivotItem);
            spec.SetContent(Resource.Id.tab2);
            TabHost.AddTab(spec);

            spec = TabHost.NewTabSpec("child3");
			spec.SetIndicator(Model.RoutesPivotItem);
            spec.SetContent(Resource.Id.tab3);
            TabHost.AddTab(spec);

			_progressBar = FindViewById<ProgressBar> (Resource.Id.progressBar);
			_searchTrainButton = FindViewById<Button>(Resource.Id.SearchTrain);
            _searchDateButton = FindViewById<Button>(Resource.Id.SearchDate);
            _searchTypeButton = FindViewById<Button>(Resource.Id.SearchType);
            _fromTextView = FindViewById<AutoCompleteTextView>(Resource.Id.FromTextView);
            _toTextView = FindViewById<AutoCompleteTextView>(Resource.Id.ToTextView);
        }
     protected override void OnStart()
        {
            _searchDateButton.Click += searchDateButton_Click;
            _searchDateButton.Text = SearchDate.ToString(DateFormat);

			TabHost.TabChanged+=tab_changed;

			_searchTrainButton.Click += searchTrainButton_Click;
            _searchTypeButton.Click += searchTypeButton_Click;
            _searchTypeButton.Text = SelectedType;
			_fromTextView.TextChanged += fromTextView_TextChange;
			_toTextView.TextChanged += toTextView_TextChange;

            base.OnStart();
        }

		void tab_changed (object sender, TabHost.TabChangeEventArgs e)
		{
			Model.RaisePropertyChanged("LastUpdateTime");
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

		private async void searchTrainButton_Click(object sender, EventArgs e)
		{
			if (!Model.IsTaskRun) {
				Model.SearchTrainCommand.Execute ();
				Task.Run (async () => {
					int i = 0;
					while (Model.IsTaskRun) {
						i+=10;
						_progressBar.Progress = i;
						await Task.Delay(new TimeSpan(800));
						if (i == 100)
							i = 0;
					}
				});
			}
		}

		private async Task setProgress()
		{
			int i = 0;
			while (Model.IsTaskRun) {
				++i;
				_progressBar.Progress = i;
				if (i == 100)
					i = 0;
			}
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