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

namespace Trains.Droid.Views
{
	[Activity(Label = "SearchView")]
	public class SearchView : MvxActivity
	{
		private Button _searchDateButton;
		private DateTime _date;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.SearchView);

			_date = DateTime.Now;

			_searchDateButton = FindViewById<Button>(Resource.Id.SearchDate);
			_searchDateButton.Click += searchDateButton_Click;
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
					return new DatePickerDialog(this, HandleSearchDateSet, _date.Year, _date.Month, _date.Day);
				default:
					return base.OnCreateDialog(id);
			}
		}

		private void HandleSearchDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			_date = e.Date;
			_searchDateButton.Text = _date.ToString();
		}
	}

	enum DialogTypes : byte
	{ 
		DatePicker = 0	
	}
}