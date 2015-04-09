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
		
		private DateTimeOffset SearchDate
		{
			get { return ((SearchViewModel)ViewModel).Datum; }
			set { ((SearchViewModel)ViewModel).Datum = value; }
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.SearchView);

			_searchDateButton = FindViewById<Button>(Resource.Id.SearchDate);
			_searchDateButton.Click += searchDateButton_Click;
			_searchDateButton.Text = SearchDate.ToString(DateFormat);
		}

		private void searchDateButton_Click(object sender, EventArgs e)
		{
			ShowDialog((int)DialogTypes.DatePicker);
		}

		protected override Dialog OnCreateDialog(int id)
		{
			switch (id)
			{ 
				case (int)DialogTypes.DatePicker:
					return new DatePickerDialog(this, HandleSearchDateSet, SearchDate.Year, SearchDate.Month - 1, SearchDate.Day);
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
		DatePicker = 0	
	}
}