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
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Trains.Droid.Views
{
	[Activity(Label = "View for MainViewModel")]
	public class MainView : MvxTabActivity
	{
        private const string DateFormat = "d";

        private Button _searchDateButton;
		private Button _searchTrainButton;
        private Button _searchTypeButton;
		private IMenuItem _updateMenuItem;
		private IMenuItem _swapMenuItem;
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



			spec = TabHost.NewTabSpec("main");
			spec.SetIndicator(Model.MainPivotItem);
            spec.SetContent(Resource.Id.tab1);
            TabHost.AddTab(spec);

            spec = TabHost.NewTabSpec("lastRoute");
			spec.SetIndicator(Model.LastSchedulePivotItem);
            spec.SetContent(Resource.Id.tab2);
            TabHost.AddTab(spec);

            spec = TabHost.NewTabSpec("routes");
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

		public override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			Window.SetTitle("Главная");
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.main_menu, menu);
			_updateMenuItem = menu.FindItem (Resource.Id.update);
			_swapMenuItem = menu.FindItem(Resource.Id.swap);
			SetAppBarVisibility (false, true);

			return base.OnPrepareOptionsMenu(menu);
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
			Model.RaiseAllPropertiesChanged ();
            base.OnStart();
        }

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.swap:
				{
					Model.SwapCommand.Execute ();
					return true;
				}
			case Resource.Id.update:
				{
					Model.UpdateLastRequestCommand.Execute ();
					return true;
				}
			case Resource.Id.help:
				{
					Model.GoToHelpCommand.Execute ();
					return true;
				}
			}
			return base.OnOptionsItemSelected(item);
		}

		void tab_changed (object sender, TabHost.TabChangeEventArgs e)
		{
			if (e.TabId == "main")
				SetAppBarVisibility (false, true);
			else if (e.TabId == "lastRoute")
				SetAppBarVisibility (true);
			else 
			{
				SetAppBarVisibility ();
				Model.RaisePropertyChanged ("LastUpdateTime");
			}
		}

		private void SetAppBarVisibility(bool update=false,bool swap=false)
		{
			_updateMenuItem.SetVisible(update);
			_swapMenuItem.SetVisible(swap);
		}

		private void fromTextView_TextChange(object sender, EventArgs e)
		{
			if (AutoCompletion == null)
				return;
			_fromTextView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, AutoCompletion);
		}
			
		private void toTextView_TextChange(object sender, EventArgs e)
		{
			if (AutoCompletion == null)
				return;
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