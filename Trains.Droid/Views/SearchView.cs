using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Trains.Core.ViewModels;

namespace Trains.Droid.Views
{
	[Activity(Label = "SearchView")]
	public class SearchView : MvxActivity
	{
		private const string DateFormat = "d";

		private Button _searchDateButton;
		private Button _searchTypeButton;
		
		private DateTimeOffset SearchDate
		{
			get { return ((SearchViewModel)ViewModel).Datum; }
			set { ((SearchViewModel)ViewModel).Datum = value; }
		}

		private List<string> SearchTypes
		{
			get { return ((SearchViewModel)ViewModel).VariantOfSearch; }
		}

		private string SelectedType
		{
			get { return ((SearchViewModel)ViewModel).SelectedVariant; }
			set { ((SearchViewModel)ViewModel).SelectedVariant = value; }
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.SearchView);

			_searchDateButton = FindViewById<Button>(Resource.Id.SearchDate);
			_searchDateButton.Click += searchDateButton_Click;
			_searchDateButton.Text = SearchDate.ToString(DateFormat);

			_searchTypeButton = FindViewById<Button>(Resource.Id.SearchType);
			_searchTypeButton.Click += searchTypeButton_Click;
			_searchTypeButton.Text = SelectedType;
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