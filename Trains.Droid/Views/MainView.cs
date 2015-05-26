using Android.App;
using Android.OS;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System;
using System.Collections.Generic;
using Trains.Core.ViewModels;
using Android.Views;
using Trains.Model.Entities;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;

namespace Trains.Droid.Views
{
	[Activity(Label = "View for MainViewModel")]
	public class MainView : MvxTabActivity
	{
        private const string DateFormat = "d";

        Button _searchDateButton;
		Button _searchTrainButton;
        Button _searchTypeButton;
		IMenuItem _updateMenuItem;
		IMenuItem _swapMenuItem;
        AutoCompleteTextView _fromTextView;
        AutoCompleteTextView _toTextView;
		ProgressBar _progressBar;

		Dictionary<int,Action> _actionBar;

		MainViewModel Model
		{
			get{ return (MainViewModel)ViewModel;}
		}

        DateTimeOffset SearchDate
        {
            get { return Model.Datum; }
            set { Model.Datum = value; }
        }

        List<string> SearchTypes
        {
            get { return Model.VariantOfSearch; }
        }

        string SelectedType
        {
            get { return Model.SelectedVariant; }
            set { Model.SelectedVariant = value; }
        }

        List<string> AutoCompletion
        {
            get { return Model.AutoSuggestions; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			this.RequestWindowFeature (WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.MainView);

			_progressBar = FindViewById<ProgressBar> (Resource.Id.progressBar);
			_searchTrainButton = FindViewById<Button>(Resource.Id.SearchTrain);
            _searchDateButton = FindViewById<Button>(Resource.Id.SearchDate);
            _searchTypeButton = FindViewById<Button>(Resource.Id.SearchType);
            _fromTextView = FindViewById<AutoCompleteTextView>(Resource.Id.FromTextView);
            _toTextView = FindViewById<AutoCompleteTextView>(Resource.Id.ToTextView);

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
        }

		void ReCreateActivity (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsDownloadRun")
			{
				var intent =	new Intent (this, typeof(MainView));
				intent.SetFlags (ActivityFlags.ClearTop);
				FinishActivity (0);
				StartActivity (intent);
			}
		}
			
		public override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			Window.SetTitle(Model.MainPivotItem);
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.main_menu, menu);
			_updateMenuItem = menu.FindItem (Resource.Id.update);
			_swapMenuItem = menu.FindItem(Resource.Id.swap);
			if (Model.AboutItems != null) 
			{
				var items = Model.AboutItems.ToList ();
				menu.FindItem (Resource.Id.rate).SetTitle (items [1].Text);
				menu.FindItem (Resource.Id.mail).SetTitle (items [2].Text);
				menu.FindItem (Resource.Id.settings).SetTitle (items [3].Text);
				menu.FindItem (Resource.Id.about).SetTitle (items [4].Text);
				menu.FindItem (Resource.Id.help).SetTitle (Model.HelpAppBar);
			}
			SetAppBarVisibility (false, true);

			return base.OnPrepareOptionsMenu(menu);
		}

     protected override void OnStart()
        {
			Model.PropertyChanged += ReCreateActivity;

			_actionBar=new Dictionary<int,Action>()
			{
				{Resource.Id.swap,()=>Model.SwapCommand.Execute ()},
				{Resource.Id.update,()=>Model.UpdateLastRequestCommand.Execute ()},
				{Resource.Id.help,()=>Model.GoToHelpCommand.Execute ()},
				{Resource.Id.settings,()=>Model.TappedAboutItemCommand.Execute (new About{Item=AboutPicture.Settings})},
			 	{Resource.Id.rate,()=>Model.TappedAboutItemCommand.Execute (new About{Item=AboutPicture.Market})},
				{Resource.Id.about,()=>Model.TappedAboutItemCommand.Execute (new About{Item=AboutPicture.AboutApp})},
				{Resource.Id.mail,()=>Model.TappedAboutItemCommand.Execute (new About{Item=AboutPicture.Mail})}
			};

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

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (!Model.IsTaskRun)
			_actionBar [item.ItemId] ();
			return true;
		}

		void tab_changed (object sender, TabHost.TabChangeEventArgs e)
		{
			if (e.TabId == "main")
				SetAppBarVisibility (false, true);
			else if (e.TabId == "lastRoute") {
				SetAppBarVisibility (true);
				Model.RaisePropertyChanged ("LastUpdateTime");
			}
			else 
				SetAppBarVisibility ();
		}

		void SetAppBarVisibility(bool update=false,bool swap=false)
		{
			_updateMenuItem.SetVisible(update);
			_swapMenuItem.SetVisible(swap);
		}

		void fromTextView_TextChange(object sender, EventArgs e)
		{
			if (AutoCompletion == null)
				return;
			_fromTextView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, AutoCompletion);
		}
			
		void toTextView_TextChange(object sender, EventArgs e)
		{
			if (AutoCompletion == null)
				return;
			_toTextView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, AutoCompletion);
		}

        void searchDateButton_Click(object sender, EventArgs e)
        {
			ShowDialog((int)DialogTypes.DatePicker);
        }

		private async void searchTrainButton_Click(object sender, EventArgs e)
		{
			if (!Model.IsTaskRun) 
				Model.SearchTrainCommand.Execute ();
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